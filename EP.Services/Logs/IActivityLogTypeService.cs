using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using System.Threading.Tasks;

namespace EP.Services.Logs
{
    public interface IActivityLogTypeService
    {
        Task<IPagedList<ActivityLogType>> FindAsync(int? page, int? size);

        Task<ActivityLogType> FindAsync(string id);

        Task<bool> UpdateAsync(string id, bool enabled);
    }
}
