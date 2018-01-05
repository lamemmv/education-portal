using EP.Data.Entities.Blobs;
using EP.Data.Paginations;
using System.Threading.Tasks;

namespace EP.Services.Blobs
{
    public interface IBlobService
    {
        Task<IPagedList<Blob>> GetChildListAsync(string id, int? page, int? size);
        
        Task<Blob> GetByIdAsync(string id);

        Task<EmbeddedBlob> GetEmbeddedBlobByIdAsync(string id);

        //Task<string> GetPhysicalPath(string id);

        bool IsFile(Blob entity);

        Task<bool> IsExistence(string parent, string name);

        //Task<bool> IsSystem(string id);

        Task<bool> HasChildren(string id);

        Task<Blob> CreateAsync(Blob entity);

        Task<bool> UpdateAsync(Blob entity);

        Task<Blob> DeleteAsync(string id);
    }
}
