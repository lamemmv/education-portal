using EP.Data.Entities.Blobs;
using EP.Data.Repositories;
using EP.Services.Utilities;
using MongoDB.Driver;
using System.IO;
using System.Threading;
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

        public async Task<byte[]> UploadFileAsync(Stream stream, string physicalPath)
        {
            const int DefaultBufferSize = 80 * 1024;
            byte[] content = null;

            using (var fileStream = new FileStream(physicalPath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream, DefaultBufferSize, default(CancellationToken));

                BinaryReader binaryReader = new BinaryReader(stream);
                content = binaryReader.ReadBytes((int)stream.Length);
            }

            return content;
        }

        public string GetRandomFileName(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            return $"{name}_{RandomUtils.Numberic(7)}{extension}";
        }
    }
}
