using EP.Data.DbContext;
using EP.Data.Entities.Blobs;
using EP.Data.Extensions;
using EP.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Blob>> GetChildListAsync(string id)
        {
            FilterDefinition<Blob> filter;

            if (string.IsNullOrWhiteSpace(id))
            {
                filter = Builders<Blob>.Filter.Exists(e => e.Parent, false);
            }
            else if (id.IsInvalidObjectId())
            {
                return Enumerable.Empty<Blob>();
            }
            else
            {
                filter = Builders<Blob>.Filter.Eq(e => e.Parent, id);
            }

            var projection = Builders<Blob>.Projection
                .Include(e => e.Id)
                .Include(e => e.Name)
                .Include(e => e.ContentType);

            return await _blobs.GetAllAsync(filter, projection: projection);
        }

        public async Task<Blob> GetByIdAsync(string id)
        {
            var projection = Builders<Blob>.Projection.Exclude(e => e.PhysicalPath);

            return await _blobs.GetByIdAsync(id, projection);
        }

        public async Task<EmbeddedBlob> GetEmbeddedBlobByIdAsync(string id)
        {
            var projection = Builders<Blob>.Projection
                .Include(e => e.Id)
                .Include(e => e.VirtualPath);

            var entity = await _blobs.GetByIdAsync(id, projection);

            return new EmbeddedBlob { Id = entity.Id, VirtualPath = entity.VirtualPath };
        }

        public async Task<string> GetPhysicalPath(string id)
        {
            var projection = Builders<Blob>.Projection.Include(e => e.PhysicalPath);
            var entity = await _blobs.GetByIdAsync(id, projection);

            return entity?.PhysicalPath;
        }

        public bool IsFile(Blob entity)
        {
            return entity != null &&
                !string.IsNullOrEmpty(entity.FileExtension) &&
                !string.IsNullOrEmpty(entity.ContentType) &&
                !string.IsNullOrEmpty(entity.VirtualPath);
        }

        public async Task<bool> ExistBlob(string parent, string name)
        {
            var filter = Builders<Blob>.Filter.Eq(e => e.Parent, parent) &
                Builders<Blob>.Filter.Eq(e => e.Name, name.ToLowerInvariant());
            var projection = Builders<Blob>.Projection.Include(e => e.Id);
            var entity = await _blobs.GetSingleAsync(filter, projection);

            return entity != null;
        }

        public async Task<Blob> CreateAsync(Blob entity)
        {
            return await _blobs.CreateAsync(entity);
        }

        public async Task<Blob> DeleteAsync(string id)
        {
            return await _blobs.DeleteAsync(id, projection: null);
        }
    }
}
