using EP.Data.DbContext;
using EP.Data.Entities.Blobs;
using EP.Data.Entities;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Constants;
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
            => await _blobs.GetByIdAsync(id);

        public async Task<EmbeddedBlob> GetEmbeddedBlobByIdAsync(string id)
        {
            var projection = Builders<Blob>.Projection
                .Include(e => e.Id)
                .Include(e => e.VirtualPath);

            var entity = await _blobs.GetByIdAsync(id, projection);

            return new EmbeddedBlob { Id = entity.Id, VirtualPath = entity.VirtualPath };
        }

        public async Task<Blob> CreateFolderAsync(Blob entity, EmbeddedUser embeddedUser, string ip)
        {
            await EnsureNotDuplicatedName(entity.Parent, entity.Name);

            var parentEntity = await GetByIdAsync(entity.Parent);

            if (parentEntity == null)
            {
                throw new BadRequestException(ApiStatusCode.Blob_InvalidParent, InvalidParentField);
            }

            var ancestors = new List<BlobAncestor>(parentEntity.Ancestors ?? new List<BlobAncestor>());
            ancestors.Add(new BlobAncestor(parentEntity.Id, parentEntity.Name));
            entity.Ancestors = ancestors;

            await _blobs.CreateAsync(entity);

            // Activity Log.
            await _activityLogService.CreateAsync(SystemKeyword.CreateBlob, entity, embeddedUser, ip);

            return entity;
        }

        public async Task<IEnumerable<string>> CreateFileAsync(IFormFile[] files, string parent, EmbeddedUser embeddedUser, string ip)
        {
            var parentEntity = string.IsNullOrWhiteSpace(parent) ?
                await GetSystemFolder(_commonFolderName) :
                await GetByIdAsync(parent);

            if (parentEntity == null)
            {
                throw new BadRequestException(ApiStatusCode.Blob_InvalidParent, InvalidParentField);
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
                throw new BadRequestException(ApiStatusCode.Blob_InvalidParent, InvalidParentField);
            }

            var ancestors = new List<BlobAncestor>(parentEntity.Ancestors ?? new List<BlobAncestor>());
            ancestors.Add(new BlobAncestor(parentEntity.Id, parentEntity.Name));

            IList<string> results = new List<string>();
            Blob entity;
            string subType, fileName, randomName,
                folderPhysicalPath, filePhysicalPath, fileVirtualPath;

            foreach (var file in files)
            {
                subType = file.GetSubTypeFromContentType();

                if (string.IsNullOrEmpty(subType))
                {
                    throw new BadRequestException(ApiStatusCode.Blob_InvalidMIMEType, "The MIME type is not valid.");
                }

                fileName =
                    ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                randomName = GetRandomFileName(fileName);

                folderPhysicalPath = Path.Combine(rootPhysicalPath, subType);
                CreateDirectoryIfNotExist(folderPhysicalPath);

                filePhysicalPath = Path.Combine(folderPhysicalPath, randomName);

                if (File.Exists(filePhysicalPath))
                {
                    throw new BadRequestException(ApiStatusCode.Blob_DuplicatedName, $"The {randomName} was existed.");
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

                // Activity Log.
                await _activityLogService.CreateAsync(SystemKeyword.CreateBlob, entity, embeddedUser, ip);

                results.Add(entity.Id);
            }

            return results;
        }

        public async Task<bool> UpdateFolderAsync(Blob entity, EmbeddedUser embeddedUser, string ip)
        {
            await EnsureNotDuplicatedName(entity.Parent, entity.Name);

            var update = Builders<Blob>.Update
                .Set(e => e.Name, entity.Name)
                .CurrentDate(e => e.UpdatedOn);

            var result = await _blobs.UpdatePartiallyAsync(entity.Id, update);

            // Activity Log.
            await _activityLogService.CreateAsync(SystemKeyword.UpdateBlob, entity, embeddedUser, ip);

            return result;
        }

        public async Task DeleteAsync(string[] ids, EmbeddedUser embeddedUser, string ip)
        {
            foreach (var id in ids)
            {
                await DeleteAsync(id, embeddedUser, ip);
            }
        }

        public async Task<bool> DeleteAsync(string id, EmbeddedUser embeddedUser, string ip)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                return false;
            }

            if (entity.IsFile() && File.Exists(entity.PhysicalPath))
            {
                File.Delete(entity.PhysicalPath);
            }
            else
            {
                await EnsureFolderIsEmpty(id);

                // System Directory.
                EnsureNotSystemFolder(entity);
            }

            var result = await _blobs.DeleteAsync(id);

            // Activity Log.
            await _activityLogService.CreateAsync(SystemKeyword.DeleteBlob, entity, embeddedUser, ip);

            return result;
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

        private async Task EnsureNotDuplicatedName(string parent, string name)
        {
            var filter = Builders<Blob>.Filter.Eq(e => e.Parent, parent) &
                Builders<Blob>.Filter.Eq(e => e.Name, name.ToLowerInvariant());
            var projection = Builders<Blob>.Projection.Include(e => e.Id);
            var entity = await _blobs.GetSingleAsync(filter, projection);

            if (entity != null)
            {
                throw new BadRequestException(
                    ApiStatusCode.Blob_DuplicatedName,
                    $"The {entity.Name} with {parent} parent was existed.");
            }
        }

        private async Task EnsureFolderIsEmpty(string id)
        {
            var filter = Builders<Blob>.Filter.Eq(e => e.Parent, id);
            var dbCount = await _blobs.CountAsync(filter);

            if (dbCount > 0)
            {
                throw new BadRequestException(ApiStatusCode.Blob_HasChildren, "Folder is not empty.");
            }
        }

        private void EnsureNotSystemFolder(Blob entity)
        {
            if (entity.IsSystemFolder())
            {
                throw new BadRequestException(ApiStatusCode.Blob_SystemDirectory, "System folder could not be deleted.");
            }
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
