using EP.Data;
using EP.Data.Entities.Blobs;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public sealed class BlobService : IBlobService
    {
        private readonly MongoDbContext _dbContext;

        public BlobService(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Blob> FindAsync(string id)
        {
            return await _dbContext.Blobs.FindAsync(id);
        }

        public async Task<Blob> CreateAsync(Blob entity)
        {
            return await _dbContext.Blobs.CreateAsync(entity);
        }

        public async Task<Blob> DeleteAsync(string id)
        {
            var deleteOpts = new FindOneAndDeleteOptions<Blob>
            {
                //Projection = Builders<Blob>.Projection.Include(e => e.Path)
            };

            return await _dbContext.Blobs.DeleteAsync(id, deleteOpts);
        }
    }
}
