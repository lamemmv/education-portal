using EP.Data.Entities.News;
using MongoDB.Driver;

namespace EP.Data.Repositories
{
    public abstract class BaseDbContext
    {
        public IMongoClient MongoClient { get; set; }

        public IMongoDatabase MongoDatabase { get; set; }

        public abstract void SetupCollections();

        public abstract IRepository<NewsItem> News { get; }
    }
}
