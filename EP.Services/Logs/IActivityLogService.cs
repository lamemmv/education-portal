using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Services.Logs
{
    public interface IActivityLogService
    {
        Task<IPagedList<ActivityLog>> FindAsync(
            DateTime createdFromUtc,
            DateTime createdToUtc,
            string userName,
            string ip,
            int? page,
            int? size);

        Task<ActivityLog> FindAsync(string id);

        Task<ActivityLog> CreateAsync(string systemKeyword, ActivityLog entity);

        Task<bool> DeleteAsync(IEnumerable<string> ids);
    }
}
