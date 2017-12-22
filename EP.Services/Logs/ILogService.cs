using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Services.Logs
{
    public interface ILogService
    {
        Task<IPagedList<Log>> GetPagedListAsync(
            DateTime createdFromUtc,
            DateTime createdToUtc,
            string[] levels,
            int? page,
            int? size);

        Task<Log> GetByIdAsync(string id);

        Task<bool> DeleteAsync(string id);

        Task<bool> DeleteAsync(IEnumerable<string> ids);
    }
}