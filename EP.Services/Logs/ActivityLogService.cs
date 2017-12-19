using EP.Data.DbContext;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Extensions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Services.Logs
{
    public sealed class ActivityLogService : IActivityLogService
    {
        private readonly IRepository<ActivityLog> _activityLogs;

        public ActivityLogService(MongoDbContext dbContext)
        {
            _activityLogs = dbContext.ActivityLogs;
        }

        public async Task<IPagedList<ActivityLog>> FindAsync(
            DateTime createdFrom,
            DateTime createdTo,
            string userName,
            string ip,
            int? page,
            int? size)
        {
            var startDate = createdFrom.StartOfDay();
            var endDate = createdTo.EndOfDay();
            var filter = Builders<ActivityLog>.Filter.Gte(e => e.CreatedOn, startDate) &
                Builders<ActivityLog>.Filter.Lte(e => e.CreatedOn, endDate);

            if (!string.IsNullOrEmpty(userName))
            {
                //filter &= Builders<ActivityLog>.Filter.Eq(e => e.sho, startDate);
            }

            if (!string.IsNullOrEmpty(ip))
            {
                filter &= Builders<ActivityLog>.Filter.Eq(e => e.IP, ip);
            }

            return await _activityLogs.FindAsync(filter, skip: page, take: size);
        }

        public async Task<ActivityLog> FindAsync(string id)
        {
            return await _activityLogs.FindAsync(id);
        }

        public async Task<bool> DeleteAsync(IEnumerable<string> ids)
        {
            var filter = Builders<ActivityLog>.Filter.In(e => e.Id, ids);

            return await _activityLogs.DeleteAsync(filter);
        }
    }
}
