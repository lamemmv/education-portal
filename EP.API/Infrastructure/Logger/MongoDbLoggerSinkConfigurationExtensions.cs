using Serilog.Configuration;
using Serilog.Events;
using Serilog;

namespace EP.API.Infrastructure.Logger
{
    public static class MongoDbLoggerSinkConfigurationExtensions
    {
        public static LoggerConfiguration MongoDb(
            this LoggerSinkConfiguration loggerConfiguration,
            string connectionString,
            string collectionName = MongoDbSink.DefaultCollectionName,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            return loggerConfiguration.Sink(
                new MongoDbSink(connectionString, collectionName),
                restrictedToMinimumLevel);
        }
    }
}
