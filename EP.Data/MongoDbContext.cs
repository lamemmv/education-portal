using EP.Data.Entities;
using EP.Data.Entities.Blobs;
using EP.Data.Entities.Logs;
using EP.Data.Entities.News;
using EP.Data.Repositories;

namespace EP.Data
{
    public sealed class MongoDbContext : BaseDbContext
    {
        private const string NewsCollectionName = "news";

        public override void SetupCollections()
        {
        }

        #region Logs

        private IRepository<ActivityLogType> _activityLogTypes;
        private IRepository<ActivityLog> _activityLogs;
        private IRepository<Log> _logs;

        public override IRepository<ActivityLogType> ActivityLogTypes
        {
            get
            {
                _activityLogTypes = _activityLogTypes ?? CreateRepository<ActivityLogType>();

                return _activityLogTypes;
            }
        }

        public override IRepository<ActivityLog> ActivityLogs
        {
            get
            {
                _activityLogs = _activityLogs ?? CreateRepository<ActivityLog>();

                return _activityLogs;
            }
        }

        public override IRepository<Log> Logs
        {
            get
            {
                _logs = _logs ?? CreateRepository<Log>();

                return _logs;
            }
        }

        #endregion

        #region Blobs

        private IRepository<Blob> _blobs;

        public override IRepository<Blob> Blobs
        {
            get
            {
                _blobs = _blobs ?? CreateRepository<Blob>();

                return _blobs;
            }
        }

        #endregion

        #region News

        private IRepository<NewsItem> _news;

        public override IRepository<NewsItem> News
        {
            get
            {
                _news = _news ?? CreateRepository<NewsItem>(NewsCollectionName);

                return _news;
            }
        }

        #endregion

        private IRepository<TEntity> CreateRepository<TEntity>(string collectionName = null) where TEntity : IEntity
        {
            collectionName = collectionName ?? typeof(TEntity).Name.ToLowerInvariant() + "s";
            var collection = MongoDatabase.GetCollection<TEntity>(collectionName);

            return new MongoRepository<TEntity>(collection);
        }
    }
}
