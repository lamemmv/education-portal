using EP.Data.Entities;
using EP.Data.Entities.Blobs;
using EP.Data.Entities.Logs;
using EP.Data.Entities.News;
using EP.Data.Repositories;

namespace EP.Data
{
    public sealed class MongoDbContext : BaseDbContext
    {
        public override void SetupCollections()
        {
        }

        #region Logs

        private IRepository<Log> _logs;

        public override IRepository<Log> Logs
        {
            get
            {
                _logs = _logs ?? CreateRepository<Log>("Logs");

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
                _blobs = _blobs ?? CreateRepository<Blob>("Blobs");

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
                _news = _news ?? CreateRepository<NewsItem>("News");

                return _news;
            }
        }

        #endregion

        private IRepository<TEntity> CreateRepository<TEntity>(string collectionName) where TEntity : IEntity
        {
            var collection = MongoDatabase.GetCollection<TEntity>(collectionName);

            return new MongoRepository<TEntity>(collection);
        }
    }
}
