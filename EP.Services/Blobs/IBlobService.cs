using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Services.Models;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public interface IBlobService
    {
        Task<IPagedList<Blob>> GetChildListAsync(string id, int? page, int? size);

        Task<Blob> GetByIdAsync(string id);

        Task<EmbeddedBlob> GetEmbeddedBlobByIdAsync(string id);

        bool IsFile(Blob entity);

        Task<ApiResponse> CreateDirectoryAsync(string parent, string name);

        Task<ApiResponse> UpdateDirectoryAsync(string id, string parent, string name);

        Task<ApiResponse> DeleteAsync(string id);
    }
}
