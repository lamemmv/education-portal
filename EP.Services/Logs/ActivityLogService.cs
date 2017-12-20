using EP.Data.DbContext;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EP.Services.Logs
{
    public sealed class ActivityLogService : IActivityLogService
    {
        private readonly IRepository<ActivityLog> _activityLogs;
        private readonly IActivityLogTypeService _activityLogTypeService;

        public ActivityLogService(
            MongoDbContext dbContext,
            IActivityLogTypeService activityLogTypeService)
        {
            _activityLogs = dbContext.ActivityLogs;
            _activityLogTypeService = activityLogTypeService;
        }

        public async Task<IPagedList<ActivityLog>> FindAsync(
            DateTime createdFromUtc,
            DateTime createdToUtc,
            string userName,
            string ip,
            int? page,
            int? size)
        {
            var filter = Builders<ActivityLog>.Filter.Gte(e => e.CreatedOn, createdFromUtc) &
                Builders<ActivityLog>.Filter.Lte(e => e.CreatedOn, createdToUtc);

            if (!string.IsNullOrEmpty(userName))
            {
                //filter &= Builders<ActivityLog>.Filter.Eq(e => e.UserName, userName.Trim());
            }

            if (!string.IsNullOrEmpty(ip))
            {
                filter &= Builders<ActivityLog>.Filter.Eq(e => e.IP, ip.Trim());
            }

            return await _activityLogs.FindAsync(filter, skip: page, take: size);
        }

        public async Task<ActivityLog> FindAsync(string id)
        {
            return await _activityLogs.FindAsync(id);
        }

        public async Task<ActivityLog> CreateAsync(string systemKeyword, ActivityLog entity)
        {
            var shortActivityLogType = await _activityLogTypeService.GetEnabledShortActivityLogTypes(systemKeyword);

            if (shortActivityLogType == null)
            {
                return null;
            }

            entity.ActivityLogType = shortActivityLogType;

            return await _activityLogs.CreateAsync(entity);
        }

        public async Task<bool> DeleteAsync(IEnumerable<string> ids)
        {
            var filter = Builders<ActivityLog>.Filter.In(e => e.Id, ids);

            return await _activityLogs.DeleteAsync(filter);
        }
    }
}
