using EP.Data.DbContext;
using EP.Data.Entities.Logs;
using EP.Data.Entities;
using EP.Data.Paginations;
using EP.Services.Caching;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EP.Services.Logs
{
    public sealed class ActivityLogService : IActivityLogService
    {
        private const string EnabledActivityLogTypes = "Cache." + nameof(EnabledActivityLogTypes);

        private readonly MongoDbContext _dbContext;
        private readonly IMemoryCacheService _memoryCacheService;

        public ActivityLogService(
            MongoDbContext dbContext,
            IMemoryCacheService memoryCacheService)
        {
            _dbContext = dbContext;
            _memoryCacheService = memoryCacheService;
        }

        #region Activity Log Type

        public async Task<IPagedList<ActivityLogType>> GetLogTypePagedListAsync(int? page, int? size)
            => await _dbContext.ActivityLogTypes.GetPagedListAsync(skip: page, take: size);

        public async Task<ActivityLogType> GetLogTypeByIdAsync(string id)
            => await _dbContext.ActivityLogTypes.GetByIdAsync(id);

        public async Task<bool> UpdateLogTypeAsync(string id, bool enabled)
        {
            var update = Builders<ActivityLogType>.Update
                .Set(e => e.Enabled, enabled);

            var result = await _dbContext.ActivityLogTypes.UpdatePartiallyAsync(id, update);

            if (result)
            {
                _memoryCacheService.Remove(EnabledActivityLogTypes);
            }

            return result;
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

            // if (!string.IsNullOrEmpty(userName))
            // {
            //     filter &= Builders<ActivityLog>.Filter.Eq(e => e.UserName, userName);
            // }

            // if (!string.IsNullOrEmpty(ip))
            // {
            //     filter &= Builders<ActivityLog>.Filter.Eq(e => e.IP, ip);
            // }

            return await _dbContext.ActivityLogs.GetPagedListAsync(filter, skip: page, take: size);
        }

        public async Task<ActivityLog> GetByIdAsync(string id)
            => await _dbContext.ActivityLogs.GetByIdAsync(id);

        public async Task<ActivityLog> CreateAsync(
            string systemKeyword,
            IEntity entity,
            EmbeddedUser embeddedUser,
            string ip)
        {
            var embeddedActivityLogType = await GetEnabledEmbeddedActivityLogTypes(systemKeyword);

            if (embeddedActivityLogType == null)
            {
                return null;
            }

            var log = new ActivityLog
            {
                EntityName = entity.GetType().FullName,
                LogValue = ObjectToJson(entity),
                IP = ip,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = embeddedUser,
                ActivityLogType = embeddedActivityLogType
            };

            return await _dbContext.ActivityLogs.CreateAsync(log);
        }

        public async Task<bool> DeleteAsync(string id)
            => await _dbContext.ActivityLogs.DeleteAsync(id);

        public async Task DeleteAsync(IEnumerable<string> ids)
        {
            var filter = Builders<ActivityLog>.Filter.In(e => e.Id, ids);
            
            await _dbContext.ActivityLogs.DeleteAsync(filter);
        }

        private async Task<EmbeddedActivityLogType> GetEnabledEmbeddedActivityLogTypes(string systemKeyword)
        {
            var enabledDictionary = await _memoryCacheService.GetOrAddSlidingExpiration(
                EnabledActivityLogTypes,
                async () =>
                {
                    var filter = Builders<ActivityLogType>.Filter.Eq(e => e.Enabled, true);
                    var projection = Builders<ActivityLogType>.Projection
                        .Include(e => e.Id)
                        .Include(e => e.SystemKeyword)
                        .Include(e => e.Name);

                    var logTypes = await _dbContext.ActivityLogTypes.GetAllAsync(filter, projection: projection);

                    return logTypes.ToDictionary(
                        kvp => kvp.SystemKeyword,
                        kvp => new EmbeddedActivityLogType(kvp.Id, kvp.Name));
                });

            return enabledDictionary == null || !enabledDictionary.TryGetValue(systemKeyword, out EmbeddedActivityLogType embeddedActivityLogType) ?
                null :
                embeddedActivityLogType;
        }

        private static string ObjectToJson(object value)
            => JsonConvert.SerializeObject(
                value,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
    }
}
