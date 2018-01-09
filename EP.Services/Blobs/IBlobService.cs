using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using EP.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public interface IBlobService
    {
        Task<IPagedList<Blob>> GetChildListAsync(string id, int? page, int? size);

        Task<Blob> GetBlobForChildListAsync(string id);

        Task<Blob> GetByIdAsync(string id);

        Task<EmbeddedBlob> GetEmbeddedBlobByIdAsync(string id);

        Task<ApiServerResult> CreateFolderAsync(Blob entity);

        Task<IEnumerable<ApiServerResult>> CreateFileAsync(string parent, IFormFile[] files);

        Task<ApiServerResult> UpdateFolderAsync(Blob entity);

        Task<IEnumerable<BlobDeleteResult>> DeleteAsync(string[] ids);

        Task<BlobDeleteResult> DeleteAsync(string id);
    }
}
