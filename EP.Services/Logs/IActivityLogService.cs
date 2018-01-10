using EP.Data.Entities.Logs;
using EP.Data.Entities;
using EP.Data.Paginations;
using EP.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EP.Services.Logs
{
    public interface IActivityLogService
    {
        Task<IPagedList<ActivityLogType>> GetLogTypePagedListAsync(int? page, int? size);

        Task<ActivityLogType> GetLogTypeByIdAsync(string id);

        Task<ApiServerResult> UpdateLogTypeAsync(string id, bool enabled);

        Task<IPagedList<ActivityLog>> GetPagedListAsync(
            DateTime createdFromUtc,
            DateTime createdToUtc,
            string userName,
            string ip,
            int? page,
            int? size);

        Task<ActivityLog> GetByIdAsync(string id);

        Task<ActivityLog> CreateAsync(
            string systemKeyword,
            IEntity entity,
            EmbeddedUser embeddedUser,
            string ip = null);

        Task<ApiServerResult> DeleteAsync(string id);

        Task<ApiServerResult> DeleteAsync(IEnumerable<string> ids);
    }
}
