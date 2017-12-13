using MongoDB.Driver;

namespace EP.Data.DbContext
{
    public class MongoDbContextConfiguration
    {
        public MongoClientSettings ClientSettings { get; set; }

        public MongoDatabaseSettings DatabaseSettings { get; set; }

        public string ConnectionString { get; set; }
    }

    public sealed class MongoDbContextConfiguration<TContext> : MongoDbContextConfiguration
        where TContext : BaseDbContext, new()
    {
    }
}
