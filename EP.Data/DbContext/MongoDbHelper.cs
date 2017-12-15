using EP.Data.Entities;
using EP.Data.Repositories;
using MongoDB.Driver;

namespace EP.Data.DbContext
{
    public static class MongoDbHelper
    {
        public static IMongoDatabase GetMongoDatabase(
            string connectionString,
            MongoClientSettings clientSettings = null,
            MongoDatabaseSettings databaseSettings = null)
        {
            MongoUrl url = new MongoUrl(connectionString);

            IMongoClient client = clientSettings == null ?
                new MongoClient(url) :
                new MongoClient(clientSettings);

            return client.GetDatabase(url.DatabaseName, databaseSettings);
        }

        public static IRepository<TEntity> CreateRepository<TEntity>(
            IMongoDatabase database,
            string collectionName = null) where TEntity : IEntity
        {
            collectionName = collectionName ?? typeof(TEntity).Name.ToLowerInvariant() + "s";
            var collection = database.GetCollection<TEntity>(collectionName);

            return new MongoRepository<TEntity>(collection);
        }
    }
}
