using EP.Data.Entities.Blobs;
using System.Linq;

namespace EP.Services.Blobs
{
    public static class BlobExtensions
    {
        public static bool IsFile(this Blob entity)
            => entity != null &&
                !string.IsNullOrEmpty(entity.RandomName) &&
                !string.IsNullOrEmpty(entity.ContentType) &&
                !string.IsNullOrEmpty(entity.PhysicalPath);

        public static bool IsSystemFolder(this Blob entity)
            => entity != null &&
                string.IsNullOrEmpty(entity.Parent) &&
                (entity.Ancestors == null || !entity.Ancestors.Any());
    }
}