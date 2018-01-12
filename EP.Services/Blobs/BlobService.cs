using EP.Data.DbContext;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Enums;
using EP.Services.Extensions;
using EP.Services.Logs;
using EP.Services.Models;
using EP.Services.Utilities;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;

namespace EP.Services.Blobs
{
    public sealed class BlobService : IBlobService
    {
        private const string InvalidParentField = "The Parent field is invalid.";

        private readonly IRepository<Blob> _blobs;
        private readonly IActivityLogService _activityLogService;
        private readonly string _commonFolderName;

        public BlobService(
            MongoDbContext dbContext,
            IActivityLogService activityLogService,
            AppSettings appSettings)
        {
            _blobs = dbContext.Blobs;
            _activityLogService = activityLogService;
            _commonFolderName = appSettings.CommonFolder;
        }

        public async Task<IPagedList<Blob>> GetChildListAsync(string id, int? page, int? size)
        {
            FilterDefinition<Blob> filter = string.IsNullOrWhiteSpace(id) ?
                Builders<Blob>.Filter.Exists(e => e.Parent, false) :
                Builders<Blob>.Filter.Eq(e => e.Parent, id);

            var projection = Builders<Blob>.Projection
                .Include(e => e.Id)
                .Include(e => e.Name)
                .Include(e => e.ContentType);

            return await _blobs.GetPagedListAsync(filter, projection: projection, skip: page, take: size);
        }

        public async Task<Blob> GetBlobForChildListAsync(string id)
        {
            var projection = Builders<Blob>.Projection
                .Include(e => e.Id)
                .Include(e => e.Name)
                .Include(e => e.Parent)
                .Include(e => e.Ancestors);

            return await _blobs.GetByIdAsync(id, projection);
        }

        public async Task<Blob> GetByIdAsync(string id)
        {
            return await _blobs.GetByIdAsync(id);
        }

        public async Task<EmbeddedBlob> GetEmbeddedBlobByIdAsync(string id)
        {
            var projection = Builders<Blob>.Projection
                .Include(e => e.Id)
                .Include(e => e.VirtualPath);

            var entity = await _blobs.GetByIdAsync(id, projection);

            return new EmbeddedBlob { Id = entity.Id, VirtualPath = entity.VirtualPath };
        }

        public async Task<ApiServerResult> CreateFolderAsync(Blob entity)
        {
            if (await IsExistence(entity.Parent, entity.Name))
            {
                return ApiServerResult.ServerError(ApiStatusCode.Blob_DuplicatedName, $"The {entity.Name} was existed.");
            }

            var parentEntity = await GetByIdAsync(entity.Parent);

            if (parentEntity == null)
            {
                return ApiServerResult.ServerError(ApiStatusCode.Blob_InvalidParent, InvalidParentField);
            }

            var ancestors = new List<BlobAncestor>(parentEntity.Ancestors ?? new List<BlobAncestor>());
            ancestors.Add(new BlobAncestor(parentEntity.Id, parentEntity.Name));
            entity.Ancestors = ancestors;

            await _blobs.CreateAsync(entity);

            return ApiServerResult.Created(entity.Id);
        }

        public async Task<IEnumerable<ApiServerResult>> CreateFileAsync(IFormFile[] files, string parent = null)
        {
            var parentEntity = string.IsNullOrWhiteSpace(parent) ?
                await GetSystemFolder(_commonFolderName) :
                await GetByIdAsync(parent);

            if (parentEntity == null)
            {
                return new List<ApiServerResult>
                {
                    ApiServerResult.ServerError(ApiStatusCode.Blob_InvalidParent, InvalidParentField)
                };
            }

            var rootVirtualPath = parentEntity.VirtualPath;
            var rootPhysicalPath = parentEntity.PhysicalPath;

            if (string.IsNullOrEmpty(rootPhysicalPath))
            {
                var ancestorId = parentEntity.Ancestors?.FirstOrDefault()?.Id;
                var ancestor = await GetByIdAsync(ancestorId);

                rootVirtualPath = ancestor?.VirtualPath;
                rootPhysicalPath = ancestor?.PhysicalPath;
            }

            if (string.IsNullOrEmpty(rootPhysicalPath))
            {
                return new List<ApiServerResult>
                {
                    ApiServerResult.ServerError(ApiStatusCode.Blob_InvalidParent, InvalidParentField)
                };
            }

            var ancestors = new List<BlobAncestor>(parentEntity.Ancestors ?? new List<BlobAncestor>());
            ancestors.Add(new BlobAncestor(parentEntity.Id, parentEntity.Name));

            IList<ApiServerResult> results = new List<ApiServerResult>();
            Blob entity;
            string subType, fileName, randomName,
                folderPhysicalPath, filePhysicalPath, fileVirtualPath;

            foreach (var file in files)
            {
                subType = file.GetSubTypeFromContentType();

                if (string.IsNullOrEmpty(subType))
                {
                    results.Add(ApiServerResult.ServerError(ApiStatusCode.Blob_InvalidMIMEType, "The MIME type is not valid."));
                    continue;
                }

                fileName =
                    ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                randomName = GetRandomFileName(fileName);

                folderPhysicalPath = Path.Combine(rootPhysicalPath, subType);
                CreateDirectoryIfNotExist(folderPhysicalPath);

                filePhysicalPath = Path.Combine(folderPhysicalPath, randomName);

                if (File.Exists(filePhysicalPath))
                {
                    results.Add(ApiServerResult.ServerError(ApiStatusCode.Blob_DuplicatedName, $"The {randomName} was existed."));
                    continue;
                }

                fileVirtualPath = string.IsNullOrEmpty(rootVirtualPath) ?
                    null :
                    $"{rootVirtualPath}/{subType}/{randomName}";

                entity = new Blob
                {
                    Name = fileName,
                    RandomName = randomName,
                    FileExtension = Path.GetExtension(fileName).ToLowerInvariant(),
                    ContentType = file.ContentType,
                    VirtualPath = fileVirtualPath,
                    PhysicalPath = filePhysicalPath,
                    Parent = parentEntity.Id,
                    Ancestors = ancestors,
                    CreatedOn = DateTime.UtcNow
                };

                await Task.WhenAll(_blobs.CreateAsync(entity), file.SaveAsAsync(entity.PhysicalPath));

                results.Add(ApiServerResult.Created(entity.Id));
            }

            return results;
        }

        public async Task<ApiServerResult> UpdateFolderAsync(Blob entity)
        {
            if (await IsExistence(entity.Parent, entity.Name))
            {
                return ApiServerResult.ServerError(ApiStatusCode.Blob_DuplicatedName, $"The {entity.Name} was existed.");
            }

            var update = Builders<Blob>.Update
                .Set(e => e.Name, entity.Name)
                .CurrentDate(e => e.UpdatedOn);

            var result = await _blobs.UpdatePartiallyAsync(entity.Id, update);

            return result ? ApiServerResult.NoContent() : ApiServerResult.NotFound();
        }

        public async Task<IEnumerable<ApiServerResult>> DeleteAsync(string[] ids)
        {
            IList<ApiServerResult> results = new List<ApiServerResult>();

            foreach (var id in ids)
            {
                results.Add(await DeleteAsync(id));
            }

            return results;
        }

        public async Task<ApiServerResult> DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                return new ApiServerResult(ApiStatusCode.NotFound, id);
            }

            if (entity.IsFile() && File.Exists(entity.PhysicalPath))
            {
                File.Delete(entity.PhysicalPath);
            }
            else
            {
                if (await HasChildren(id))
                {
                    return new ApiServerResult(ApiStatusCode.Blob_HasChildren, id, "Folder is not empty.");
                }

                // System Directory.
                if (entity.IsSystemFolder())
                {
                    return new ApiServerResult(ApiStatusCode.Blob_SystemDirectory, id, "System folder could not be deleted.");
                }
            }

            var result = await _blobs.DeleteAsync(id);

            return result ? new ApiServerResult(id: id) : new ApiServerResult(ApiStatusCode.NotFound, id);
        }

        private async Task<Blob> GetSystemFolder(string name)
        {
            var filter = Builders<Blob>.Filter.Eq(e => e.Name, _commonFolderName) &
                Builders<Blob>.Filter.Exists(e => e.Parent, false) &
                Builders<Blob>.Filter.Exists(e => e.Ancestors, false);
            var projection = Builders<Blob>.Projection
                .Include(e => e.Id)
                .Include(e => e.Name)
                .Include(e => e.VirtualPath)
                .Include(e => e.PhysicalPath);
            var commonBlob = await _blobs.GetSingleAsync(filter, projection);

            return commonBlob;
        }

        private async Task<bool> IsExistence(string parent, string name)
        {
            var filter = Builders<Blob>.Filter.Eq(e => e.Parent, parent) &
                Builders<Blob>.Filter.Eq(e => e.Name, name.ToLowerInvariant());
            var projection = Builders<Blob>.Projection.Include(e => e.Id);
            var entity = await _blobs.GetSingleAsync(filter, projection);

            return entity != null;
        }

        private async Task<bool> HasChildren(string id)
        {
            var filter = Builders<Blob>.Filter.Eq(e => e.Parent, id);

            return await _blobs.CountAsync(filter) > 0;
        }

        private static string GetRandomFileName(string fileName, int size = 8)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);

            return $"{name}_{RandomUtils.Numberic(size)}{ext}";
        }

        private static void CreateDirectoryIfNotExist(string name)
        {
            if (!Directory.Exists(name))
            {
                Directory.CreateDirectory(name);
            }
        }
    }
}
