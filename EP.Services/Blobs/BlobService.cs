using EP.Data.DbContext;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Enums;
using EP.Services.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public sealed class BlobService : IBlobService
    {
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

        public async Task<ApiResponse> CreateDirectoryAsync(string parent, string name)
        {
            var response = await ValidateDirectory(parent, name);

            if (response.StatusCode != (int)ApiStatusCode.OK)
            {
                return response;
            }

            var entity = new Blob
            {
                Name = name.Trim(),
                Parent = parent,
                CreatedOn = DateTime.UtcNow
            };

            await _blobs.CreateAsync(entity);

            return ApiResponse.Created(entity.Id);
        }

        public async Task<ApiResponse> UpdateDirectoryAsync(string id, string parent, string name)
        {
            var response = await ValidateDirectory(parent, name);

            if (response.StatusCode != (int)ApiStatusCode.OK)
            {
                return response;
            }

            var update = Builders<Blob>.Update
                .Set(e => e.Parent, parent)
                .Set(e => e.Name, name.Trim())
                .CurrentDate(e => e.UpdatedOn);

            var result = await _blobs.UpdatePartiallyAsync(id, update);

            return result ? ApiResponse.NoContent() : ApiResponse.NotFound();
        }

        public async Task<ApiResponse> DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                return ApiResponse.NotFound();
            }

            if (IsFile(entity))
            {
                if (System.IO.File.Exists(entity.PhysicalPath))
                {
                    System.IO.File.Delete(entity.PhysicalPath);
                }
            }
            else
            {
                if (await HasChildren(id))
                {
                    return ApiResponse.BadRequest(nameof(id), $"The {id} has sub directories or files.");
                }

                // System Directory.
                if (string.IsNullOrEmpty(entity.Parent))
                {
                    return ApiResponse.BadRequest(nameof(id), $"The {id} is system directory.");
                }
            }

            var result = await _blobs.DeleteAsync(id);

            return result ? ApiResponse.NoContent() : ApiResponse.NotFound();
        }

        private async Task<ApiResponse> ValidateDirectory(string parent, string name)
        {
            var parentEntity = await GetByIdAsync(parent);

            if (parentEntity == null)
            {
                return ApiResponse.BadRequest(nameof(parent), $"The {parent} is invalid.");
            }

            string directoryName = name.Trim();

            if (await IsExistence(parent, directoryName))
            {
                return ApiResponse.BadRequest(nameof(name), $"{directoryName} is existed.");
            }

            return ApiResponse.OK();
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
