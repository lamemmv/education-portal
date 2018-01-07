using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public interface IBlobService
    {
        Task<IPagedList<Blob>> GetChildListAsync(string id, int? page, int? size);

        Task<Blob> GetByIdAsync(string id);

        Task<EmbeddedBlob> GetEmbeddedBlobByIdAsync(string id);

        bool IsFile(Blob entity);

        Task<ApiServerResult> CreateDirectoryAsync(Blob entity);

        Task<ApiServerResult> CreateFileAsync(string parent, IFormFile[] files);

        Task<ApiServerResult> UpdateDirectoryAsync(Blob entity);

        Task<ApiServerResult> DeleteAsync(string id);
    }
}
