using EP.Data.Entities.Blobs;
using EP.Data.Entities.Logs;
using EP.Data.Entities.News;
using EP.Data.Repositories;

namespace EP.Data.DbContext
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
                _activityLogTypes = _activityLogTypes ??
                    MongoDbHelper.CreateRepository<ActivityLogType>(MongoDatabase);

                return _activityLogTypes;
            }
        }

        public override IRepository<ActivityLog> ActivityLogs
        {
            get
            {
                _activityLogs = _activityLogs ??
                    MongoDbHelper.CreateRepository<ActivityLog>(MongoDatabase);

                return _activityLogs;
            }
        }

        public override IRepository<Log> Logs
        {
            get
            {
                _logs = _logs ?? MongoDbHelper.CreateRepository<Log>(MongoDatabase);

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
                _blobs = _blobs ?? MongoDbHelper.CreateRepository<Blob>(MongoDatabase);

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
                _news = _news ??
                    MongoDbHelper.CreateRepository<NewsItem>(MongoDatabase, NewsCollectionName);

                return _news;
            }
        }

        #endregion
    }
}
