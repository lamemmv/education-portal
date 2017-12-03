using EP.Data.Entities.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public interface IBlobService
    {
        Task<Blob> FindAsync(string id);

        Task<Blob> CreateAsync(Blob entity);

        Task<Blob> DeleteAsync(string id);

        Task<byte[]> UploadFileAsync(Stream stream, string physicalPath);

        string GetRandomFileName(string fileName);
    }
}
