using EP.Data;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using MongoDB.Driver;
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

        public async Task<IPagedList<Blob>> FindAsync(int page, int size)
        {
            return await _blobs.FindAsync(page, size);
        }

        public async Task<Blob> FindAsync(string id)
        {
            return await _blobs.FindAsync(id);
        }

        public async Task<Blob> CreateAsync(Blob entity)
        {
            return await _blobs.CreateAsync(entity);
        }

        public async Task<Blob> DeleteAsync(string id)
        {
            var deleteOpts = new FindOneAndDeleteOptions<Blob>
            {
                //Projection = Builders<Blob>.Projection.Include(e => e.Path)
            };

            return await _blobs.DeleteAsync(id, deleteOpts);
        }
    }
}
