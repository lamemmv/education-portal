using EP.Data.DbContext;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using EP.Services.Caching;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EP.Services.Logs
{
    public sealed class ActivityLogTypeService : IActivityLogTypeService
    {
        private const string EnabledActivityLogTypesKey = "Cache.EnabledActivityLogTypes";

        private readonly IRepository<ActivityLogType> _activityLogTypes;
        private readonly IMemoryCacheService _memoryCacheService;

        public ActivityLogTypeService(
            MongoDbContext dbContext,
            IMemoryCacheService memoryCacheService)
        {
            _activityLogTypes = dbContext.ActivityLogTypes;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<IPagedList<ActivityLogType>> FindAsync(int? page, int? size)
        {
            return await _activityLogTypes.FindAsync(skip: page, take: size);
        }

        public async Task<ActivityLogType> FindAsync(string id)
        {
            return await _activityLogTypes.FindAsync(id);
        }

        public async Task<ShortActivityLogType> GetEnabledShortActivityLogTypes(string systemKeyword)
        {
            ShortActivityLogType shortActivityLogType;

            var enabledDictionary = await _memoryCacheService.GetSlidingExpiration(
                EnabledActivityLogTypesKey,
                GetEnabledShortActivityLogTypes);

            if (enabledDictionary == null || !enabledDictionary.TryGetValue(systemKeyword, out shortActivityLogType))
            {
                return null;
            }

            return shortActivityLogType;
        }

        public async Task<ActivityLogType> UpdateAsync(string id, bool enabled)
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
                _memoryCacheService.Remove(EnabledActivityLogTypesKey);
            }

            return oldEntity;
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
