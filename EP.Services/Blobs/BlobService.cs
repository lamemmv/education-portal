using EP.Data.DbContext;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using MongoDB.Driver;
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

        public async Task<IPagedList<Blob>> GetPagedListAsync(string[] fileExtensions, int? page, int? size)
        {
            var filter = fileExtensions == null || fileExtensions.Length == 0 ?
                Builders<Blob>.Filter.Empty :
                Builders<Blob>.Filter.In(e => e.FileExtension, fileExtensions.Select(ext => ext.ToLowerInvariant()));

            var projection = Builders<Blob>.Projection.Exclude(e => e.PhysicalPath);

            return await _blobs.GetPagedListAsync(filter, projection: projection, skip: page, take: size);
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

            var entity = await GetByIdAsync(id);

            return new EmbeddedBlob { Id = entity.Id, VirtualPath = entity.VirtualPath };
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
