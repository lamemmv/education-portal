using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public interface IBlobService
    {
        Task<IPagedList<Blob>> FindAsync(string fileExtension, int? page, int? size);
        
        Task<Blob> FindAsync(string id);

        Task<Blob> CreateAsync(Blob entity);

        Task<Blob> DeleteAsync(string id);

        string GetServerUploadPathDirectory(string physicalPath, string contentType);
    }
}
