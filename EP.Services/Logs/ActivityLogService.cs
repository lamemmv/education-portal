using EP.Data.DbContext;
using EP.Data.Entities;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Services.Caching;
using EP.Services.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        {
            return await _dbContext.ActivityLogTypes.GetPagedListAsync(skip: page, take: size);
        }

        public async Task<ActivityLogType> GetLogTypeByIdAsync(string id)
        {
            return await _dbContext.ActivityLogTypes.GetByIdAsync(id);
        }

        public async Task<ApiServerResult> UpdateLogTypeAsync(string id, bool enabled)
        {
            var update = Builders<ActivityLogType>.Update
                .Set(e => e.Enabled, enabled)
                .CurrentDate(e => e.UpdatedOn);

            if (!await _dbContext.ActivityLogTypes.UpdatePartiallyAsync(id, update))
            {
                return ApiServerResult.NotFound();
            }

            _memoryCacheService.Remove(EnabledActivityLogTypes);

            return ApiServerResult.NoContent();
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

            return await _dbContext.ActivityLogs.GetPagedListAsync(filter, skip: page, take: size);
        }

        public async Task<ActivityLog> GetByIdAsync(string id)
        {
            return await _dbContext.ActivityLogs.GetByIdAsync(id);
        }

        public async Task<ActivityLog> CreateAsync(
            string systemKeyword,
            IEntity entity,
            EmbeddedUser embeddedUser,
            string ip = null)
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
                ActivityLogType = embeddedActivityLogType,
                Creator = embeddedUser,
                CreatedOn = DateTime.UtcNow
            };

            return await _dbContext.ActivityLogs.CreateAsync(log);
        }

        public async Task<ApiServerResult> DeleteAsync(string id)
        {
            var result = await _dbContext.ActivityLogs.DeleteAsync(id);

            return result ? ApiServerResult.NoContent() : ApiServerResult.NotFound();
        }

        public async Task<ApiServerResult> DeleteAsync(IEnumerable<string> ids)
        {
            var filter = Builders<ActivityLog>.Filter.In(e => e.Id, ids);
            var result = await _dbContext.ActivityLogs.DeleteAsync(filter);

            return result ? ApiServerResult.NoContent() : ApiServerResult.NotFound();
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
        {
            return JsonConvert.SerializeObject(
                value,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
