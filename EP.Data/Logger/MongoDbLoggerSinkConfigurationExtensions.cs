using EP.Data.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog.Configuration;
using Serilog.Events;
using Serilog;

namespace EP.Data.Logger
{
    public static class MongoDbLoggerSinkConfigurationExtensions
    {
        private const string LogsCollectionName = "Logs";

        public static LoggerConfiguration MongoDb(
            this LoggerSinkConfiguration loggerConfiguration,
            string connectionString,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            return loggerConfiguration.Sink(
                new MongoDbSink(GetMongoCollection(connectionString)),
                restrictedToMinimumLevel);
        }

        private static IMongoCollection<BsonDocument> GetMongoCollection(string connectionString)
        {
            return MongoDbHelper
                .GetMongoDatabase(connectionString)
                .GetCollection<BsonDocument>(LogsCollectionName);
        }
    }
}
