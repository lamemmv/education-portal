using EP.Data;
using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using MongoDB.Driver;
using System;
using System.IO;
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

        public async Task<IPagedList<Blob>> FindAsync(string[] fileExtensions, int? page, int? size)
        {
            var filter = fileExtensions == null || fileExtensions.Length == 0 ?
                Builders<Blob>.Filter.Empty :
                Builders<Blob>.Filter.In(e => e.FileExtension, fileExtensions.Select(ext => ext.ToLowerInvariant()));

            var project = Builders<Blob>.Projection
                .Exclude(e => e.PhysicalPath);

            return await _blobs.FindAsync(filter: filter, project: project, skip: page, take: size);
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
            return await _blobs.DeleteAsync(id, options: null);
        }

        public string GetServerUploadPathDirectory(string physicalPath, string contentType)
        {
            CheckAndCreateDirectory(physicalPath);

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                var types = contentType.Split('/', StringSplitOptions.RemoveEmptyEntries);

                if (types.Length > 0 && !string.IsNullOrWhiteSpace(types[0]))
                {
                    physicalPath = Path.Combine(physicalPath, types[0]);

                    CheckAndCreateDirectory(physicalPath);
                }
            }

            return physicalPath;
        }

        private static void CheckAndCreateDirectory(string physicalPath)
        {
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
            }
        }
    }
}
