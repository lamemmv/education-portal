using EP.Data.DbContext;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Caching;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IPagedList<ActivityLogType>> GetLogTypePagedListAsync(int? page, int? size)
        {
            return await _activityLogTypes.GetPagedListAsync(skip: page, take: size);
        }

        public async Task<ActivityLogType> GetLogTypeByIdAsync(string id)
        {
            return await _activityLogTypes.GetByIdAsync(id);
        }

        public async Task<ActivityLogType> UpdateLogTypeAsync(string id, bool enabled)
        {
            var update = Builders<ActivityLogType>.Update
                .Set(e => e.Enabled, enabled)
                .CurrentDate(e => e.UpdatedOn);

            var oldEntity = await _activityLogTypes.UpdatePartiallyAsync(id, update, projection: null);

            if (oldEntity != null)
            {
                _memoryCacheService.Remove(EnabledActivityLogTypes);
            }

            return oldEntity;
        }

        #endregion

        public async Task<IPagedList<ActivityLog>> GetPagedListAsync(
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
                //filter &= Builders<ActivityLog>.Filter.Eq(e => e.UserName, userName);
            }

            if (!string.IsNullOrEmpty(ip))
            {
                filter &= Builders<ActivityLog>.Filter.Eq(e => e.IP, ip);
            }

            return await _activityLogs.GetPagedListAsync(filter, skip: page, take: size);
        }

        public async Task<ActivityLog> GetByIdAsync(string id)
        {
            return await _activityLogs.GetByIdAsync(id);
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

        public async Task<bool> DeleteAsync(string id)
        {
            return await _activityLogs.DeleteAsync(id);
        }

        public async Task<bool> DeleteAsync(IEnumerable<string> ids)
        {
            var filter = Builders<ActivityLog>.Filter.In(e => e.Id, ids);

            return await _activityLogs.DeleteAsync(filter);
        }

        private async Task<EmbeddedActivityLogType> GetEnabledShortActivityLogTypes(string systemKeyword)
        {
            var enabledDictionary = await _memoryCacheService.GetSlidingExpiration(
                EnabledActivityLogTypes,
                GetEnabledShortActivityLogTypes);

            return enabledDictionary == null || !enabledDictionary.TryGetValue(systemKeyword, out EmbeddedActivityLogType shortActivityLogType) ?
                null :
                shortActivityLogType;
        }

        private async Task<IDictionary<string, EmbeddedActivityLogType>> GetEnabledShortActivityLogTypes()
        {
            var filter = Builders<ActivityLogType>.Filter.Eq(e => e.Enabled, true);
            var projection = Builders<ActivityLogType>.Projection
                .Include(e => e.SystemKeyword)
                .Include(e => e.Name);

            var logTypes = await _activityLogTypes.GetAllAsync(filter, sort: null, projection: projection);

            return logTypes.ToDictionary(
                kvp => kvp.SystemKeyword,
                kvp => new EmbeddedActivityLogType { SystemKeyword = kvp.SystemKeyword, Name = kvp.Name });
        }
    }
}
