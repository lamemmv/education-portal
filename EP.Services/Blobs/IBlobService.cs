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

        bool IsFile(Blob entity);

        Task<string> GetPhysicalPath(string id);

        Task<bool> ExistBlob(string parent, string name);

        Task<Blob> CreateAsync(Blob entity);

        Task<Blob> DeleteAsync(string id);
    }
}
