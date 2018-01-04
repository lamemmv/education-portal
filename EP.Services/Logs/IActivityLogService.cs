using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EP.Services.Logs
{
    public interface IActivityLogService
    {
        Task<IPagedList<ActivityLogType>> GetLogTypePagedListAsync(int? page, int? size);

        Task<ActivityLogType> GetLogTypeByIdAsync(string id);

        Task<long> CountLogTypeAsync();

        Task CreateLogTypeAsync(IEnumerable<ActivityLogType> entities);

        Task<ActivityLogType> UpdateLogTypeAsync(string id, bool enabled);

        Task<bool> DeleteLogTypeAsync();
        
        Task<IPagedList<ActivityLog>> GetPagedListAsync(
            DateTime createdFromUtc,
            DateTime createdToUtc,
            string userName,
            string ip,
            int? page,
            int? size);

        Task<ActivityLog> GetByIdAsync(string id);

        Task<ActivityLog> CreateAsync(string systemKeyword, ActivityLog entity);

        Task<bool> DeleteAsync(string id);

        Task<bool> DeleteAsync(IEnumerable<string> ids);
    }
}
