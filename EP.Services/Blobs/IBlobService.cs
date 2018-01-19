using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
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

        Task<Blob> CreateFolderAsync(Blob entity);

        Task<IEnumerable<string>> CreateFileAsync(IFormFile[] files, string parent = null);

        Task<bool> UpdateFolderAsync(Blob entity);

        Task DeleteAsync(string[] ids);

        Task<bool> DeleteAsync(string id);
    }
}
