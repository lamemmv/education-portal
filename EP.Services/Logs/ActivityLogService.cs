using EP.Data.DbContext;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Caching;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EP.Services.Logs
{
    public sealed class ActivityLogService : IActivityLogService
    {
        private const string EnabledActivityLogTypes = "Cache." + nameof(EnabledActivityLogTypes);

        private readonly IRepository<ActivityLogType> _activityLogTypes;
        private readonly IRepository<ActivityLog> _activityLogs;
        private readonly IMemoryCacheService _memoryCacheService;

        public ActivityLogService(
            MongoDbContext dbContext,
            IMemoryCacheService memoryCacheService)
        {
            _activityLogTypes = dbContext.ActivityLogTypes;
            _activityLogs = dbContext.ActivityLogs;
            _memoryCacheService = memoryCacheService;
        }

        #region Activity Log Type

        public async Task<IPagedList<ActivityLogType>> FindLogTypeAsync(int? page, int? size)
        {
            return await _activityLogTypes.FindAsync(skip: page, take: size);
        }

        public async Task<ActivityLogType> FindLogTypeAsync(string id)
        {
            return await _activityLogTypes.FindAsync(id);
        }

        public async Task<ActivityLogType> UpdateLogTypeAsync(string id, bool enabled)
        {
            var update = Builders<ActivityLogType>.Update
                .Set(e => e.Enabled, enabled)
                .CurrentDate(e => e.UpdatedOn);

            var options = new FindOneAndUpdateOptions<ActivityLogType, ActivityLogType>
            {
                ReturnDocument = ReturnDocument.Before
            };

            var oldEntity = await _activityLogTypes.UpdatePartiallyAsync(id, update, options);

            if (oldEntity != null)
            {
                _memoryCacheService.Remove(EnabledActivityLogTypes);
            }

            return oldEntity;
        }

        #endregion

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
            var shortActivityLogType = await GetEnabledShortActivityLogTypes(systemKeyword);

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

        private async Task<ShortActivityLogType> GetEnabledShortActivityLogTypes(string systemKeyword)
        {
            ShortActivityLogType shortActivityLogType;

            var enabledDictionary = await _memoryCacheService.GetSlidingExpiration(
                EnabledActivityLogTypes,
                GetEnabledShortActivityLogTypes);

            if (enabledDictionary == null || !enabledDictionary.TryGetValue(systemKeyword, out shortActivityLogType))
            {
                return null;
            }

            return shortActivityLogType;
        }

        private async Task<IDictionary<string, ShortActivityLogType>> GetEnabledShortActivityLogTypes()
        {
            var filter = Builders<ActivityLogType>.Filter.Eq(e => e.Enabled, true);
            var project = Builders<ActivityLogType>.Projection.Exclude(e => e.Enabled);
            var logTypes = await _activityLogTypes.FindAsync(filter, sort: null, project: project);

            return logTypes.ToDictionary(
                kvp => kvp.SystemKeyword,
                kvp => new ShortActivityLogType { Id = kvp.Id, SystemKeyword = kvp.SystemKeyword, Name = kvp.Name });
        }
    }
}
