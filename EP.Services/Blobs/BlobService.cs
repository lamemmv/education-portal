using EP.Data.DbContext;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Enums;
using EP.Services.Extensions;
using EP.Services.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public sealed class BlobService : IBlobService
    {
        private const string InvalidParentField = "The Parent field is invalid.";
        private readonly static IDictionary<string, string> AcceptableTypes = new Dictionary<string, string>
        {
            { "image/gif", "image" },
            { "image/png", "image" },
            { "image/jpeg", "image" },
            { "application/octet-stream", "application" },
            { "application/pdf", "application" }
        };

        private readonly IRepository<Blob> _blobs;

        public BlobService(MongoDbContext dbContext)
        {
            _blobs = dbContext.Blobs;
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

        public bool IsFile(Blob entity)
        {
            return entity != null &&
                !string.IsNullOrEmpty(entity.FileExtension) &&
                !string.IsNullOrEmpty(entity.ContentType) &&
                !string.IsNullOrEmpty(entity.VirtualPath);
        }

        public async Task<ApiServerResult> CreateFolderAsync(Blob entity)
        {
            if (await IsExistence(entity.Parent, entity.Name))
            {
                return ApiServerResult.ServerError(ApiStatusCode.Blob_DuplicatedName, $"The {entity.Name} is existed.");
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

        public async Task<IEnumerable<ApiServerResult>> CreateFileAsync(string parent, IFormFile[] files)
        {
            IList<ApiServerResult> results = new List<ApiServerResult>();
            var parentEntity = await GetByIdAsync(parent);

            if (parentEntity == null)
            {
                results.Add(ApiServerResult.ServerError(ApiStatusCode.Blob_InvalidParent, InvalidParentField));
                return results;
            }

            string rootVirtualPath = parentEntity.VirtualPath;
            string rootPhysicalPath = parentEntity.PhysicalPath;

            if (string.IsNullOrEmpty(rootVirtualPath) || string.IsNullOrEmpty(rootPhysicalPath))
            {
                var ancestorId = parentEntity.Ancestors?.FirstOrDefault()?.Id;
                var ancestor = await GetByIdAsync(ancestorId);

                rootVirtualPath = ancestor?.VirtualPath;
                rootPhysicalPath = ancestor?.PhysicalPath;
            }

            if (string.IsNullOrEmpty(rootVirtualPath) || string.IsNullOrEmpty(rootPhysicalPath))
            {
                results.Add(ApiServerResult.ServerError(ApiStatusCode.Blob_InvalidParent, InvalidParentField));
                return results;
            }

            Blob entity;
            string firstMimeType, folderPhysicalPath;
            var contentTypeGroups = files.ToLookup(kvp => kvp.ContentType, kvp => kvp);

            foreach (var group in contentTypeGroups)
            {
                if (!AcceptableTypes.TryGetValue(group.Key, out firstMimeType))
                {
                    results.Add(ApiServerResult.ServerError
                        (ApiStatusCode.Blob_InvalidMIMEType, $"The MIME type {group.Key} is not valid."));
                }
                else
                {
                    foreach (var file in group)
                    {
                        string fileName =
                            ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');

                        if (await IsExistence(parent, fileName))
                        {
                            results.Add(ApiServerResult.ServerError
                                (ApiStatusCode.Blob_DuplicatedName, $"The {fileName} is existed."));
                        }
                        else
                        {
                            folderPhysicalPath = Path.Combine(rootPhysicalPath, firstMimeType);

                            if (!Directory.Exists(folderPhysicalPath))
                            {
                                Directory.CreateDirectory(folderPhysicalPath);
                            }

                            entity = new Blob
                            {
                                Name = fileName,
                                FileExtension = Path.GetExtension(fileName).ToLowerInvariant(),
                                ContentType = file.ContentType,
                                VirtualPath = $"{rootVirtualPath}/{firstMimeType}/{fileName}",
                                PhysicalPath = Path.Combine(folderPhysicalPath, fileName),
                                Parent = parent,
                                Ancestors = parentEntity.Ancestors,
                                CreatedOn = DateTime.UtcNow
                            };

                            if (File.Exists(entity.PhysicalPath))
                            {
                                results.Add(ApiServerResult.ServerError
                                    (ApiStatusCode.Blob_DuplicatedName, $"The {fileName} is existed."));
                            }
                            else
                            {
                                await Task.WhenAll(_blobs.CreateAsync(entity), file.SaveAsAsync(entity.PhysicalPath));

                                results.Add(ApiServerResult.Created(entity.Id));
                            }
                        }
                    }
                }
            }

            return results;
        }

        public async Task<ApiServerResult> UpdateFolderAsync(Blob entity)
        {
            if (await IsExistence(entity.Parent, entity.Name))
            {
                return ApiServerResult.ServerError(ApiStatusCode.Blob_DuplicatedName, $"The {entity.Name} is existed.");
            }

            var update = Builders<Blob>.Update
                .Set(e => e.Name, entity.Name)
                .CurrentDate(e => e.UpdatedOn);

            var result = await _blobs.UpdatePartiallyAsync(entity.Id, update);

            return result ? ApiServerResult.NoContent() : ApiServerResult.NotFound();
        }

        public async Task<ApiServerResult> DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                return ApiServerResult.NotFound();
            }

            if (IsFile(entity) && File.Exists(entity.PhysicalPath))
            {
                File.Delete(entity.PhysicalPath);
            }
            else
            {
                if (await HasChildren(id))
                {
                    return ApiServerResult.ServerError(ApiStatusCode.Blob_HasChildren, $"The {id} has sub directories or files.");
                }

                // System Directory.
                if (string.IsNullOrEmpty(entity.Parent))
                {
                    return ApiServerResult.ServerError(ApiStatusCode.Blob_SystemDirectory, $"The {id} is system directory.");
                }
            }

            var result = await _blobs.DeleteAsync(id);

            return result ? ApiServerResult.NoContent() : ApiServerResult.NotFound();
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
    }
}
