using EP.Data.DbContext;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EP.Services.Logs
{
    public sealed class LogService : ILogService
    {
        private readonly IRepository<Log> _logs;

        public LogService(MongoDbContext dbContext)
        {
            _logs = dbContext.Logs;
        }

        public async Task<IPagedList<Log>> GetPagedListAsync(
            DateTime createdFromUtc,
            DateTime createdToUtc,
            string[] levels,
            int? page,
            int? size)
        {
            var filter = Builders<Log>.Filter.Gte(e => e.CreatedOn, createdFromUtc) &
                Builders<Log>.Filter.Lte(e => e.CreatedOn, createdToUtc);

            if (levels != null && levels.Length > 0)
            {
                filter &= Builders<Log>.Filter.In(e => e.Level, levels.Select(lv => lv.ToLowerInvariant()));
            }

            return await _logs.GetPagedListAsync(filter, skip: page, take: size);
        }

        public async Task<Log> GetByIdAsync(string id)
            => await _logs.GetByIdAsync(id);

        public async Task<bool> DeleteAsync(string id)
            => await _logs.DeleteAsync(id);

        public async Task DeleteAsync(IEnumerable<string> ids)
        {
            var filter = Builders<Log>.Filter.In(e => e.Id, ids);

            await _logs.DeleteAsync(filter);
        }
    }
}