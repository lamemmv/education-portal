using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public interface IBlobService
    {
        Task<IPagedList<Blob>> GetPagedListAsync(string[] fileExtensions, int? page, int? size);
        
        Task<Blob> GetByIdAsync(string id);

        Task<EmbeddedBlob> GetEmbeddedBlobByIdAsync(string id);

        Task<Blob> CreateAsync(Blob entity);

        Task<Blob> DeleteAsync(string id);
    }
}
