using EP.Data.Entities.Blobs;

namespace EP.Services.Blobs
{
    public static class BlobExtensions
    {
        public static bool IsFile(this Blob entity)
        {
            return entity != null &&
                !string.IsNullOrEmpty(entity.RandomName) &&
                !string.IsNullOrEmpty(entity.ContentType) &&
                !string.IsNullOrEmpty(entity.PhysicalPath);
        }
    }
}