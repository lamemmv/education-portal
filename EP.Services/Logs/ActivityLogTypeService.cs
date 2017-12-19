using EP.Data.DbContext;
using EP.Data.Entities.Logs;
using EP.Data.Paginations;
using EP.Data.Repositories;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace EP.Services.Logs
{
    public sealed class ActivityLogTypeService : IActivityLogTypeService
    {
        private readonly IRepository<ActivityLogType> _activityLogTypes;

        public ActivityLogTypeService(MongoDbContext dbContext)
        {
            _activityLogTypes = dbContext.ActivityLogTypes;
        }

        public async Task<IPagedList<ActivityLogType>> FindAsync(int? page, int? size)
        {
            return await _activityLogTypes.FindAsync(skip: page, take: size);
        }

        public async Task<ActivityLogType> FindAsync(string id)
        {
            return await _activityLogTypes.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(string id, bool enabled)
        {
            var update = Builders<ActivityLogType>.Update
                .Set(e => e.Enabled, enabled)
                .CurrentDate(e => e.UpdatedOn);

            return await _activityLogTypes.UpdatePartiallyAsync(id, update);
        }
    }
}
