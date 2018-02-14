using EP.Data.Entities.Blobs;
using EP.Data.Entities;
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

        Task<Blob> CreateFolderAsync(Blob entity, EmbeddedUser embeddedUser, string ip);

        Task<IEnumerable<string>> CreateFileAsync(IFormFile[] files, string parent, EmbeddedUser embeddedUser, string ip);

        Task<bool> UpdateFolderAsync(Blob entity, EmbeddedUser embeddedUser, string ip);

        Task DeleteAsync(string[] ids, EmbeddedUser embeddedUser, string ip);

        Task<bool> DeleteAsync(string id, EmbeddedUser embeddedUser, string ip);
    }
}
