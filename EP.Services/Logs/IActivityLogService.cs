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
            DateTime createdFrom,
            DateTime createdTo,
            string userName,
            string ip,
            int? page,
            int? size);

        Task<ActivityLog> FindAsync(string id);

        Task<bool> DeleteAsync(IEnumerable<string> ids);
    }
}
