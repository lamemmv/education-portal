using EP.Data.Entities.Blobs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public interface IBlobService
    {
        Task<IEnumerable<Blob>> GetChildListAsync(string id);
        
        Task<Blob> GetByIdAsync(string id);

        Task<EmbeddedBlob> GetEmbeddedBlobByIdAsync(string id);

        Task<string> GetPhysicalPath(string id);

        bool IsFile(Blob entity);

        Task<bool> ExistBlob(string parent, string name);

        Task<Blob> CreateAsync(Blob entity);

        Task<Blob> UpdateAsync(Blob entity);

        Task<Blob> DeleteAsync(string id);
    }
}
