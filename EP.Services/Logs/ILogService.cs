using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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

        Task DeleteAsync(IEnumerable<string> ids);
    }
}