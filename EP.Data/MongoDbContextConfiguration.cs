using EP.Data.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace EP.Data
{
    public class MongoDbContextConfiguration
    {
        private ConnectionString _connectionString;

        public MongoClientSettings ClientSettings { get; set; }

        public MongoDatabaseSettings DatabaseSettings { get; set; }

        public string ConnectionString
        {
            get
            {
                return _connectionString?.ToString();
            }
            set
            {
                _connectionString = new ConnectionString(value);
                DatabaseName = DatabaseName ?? _connectionString.DatabaseName;
            }
        }

        public string DatabaseName { get; set; }
    }

    public sealed class MongoDbContextConfiguration<TContext> : MongoDbContextConfiguration
        where TContext : BaseDbContext, new()
    {
    }
}
