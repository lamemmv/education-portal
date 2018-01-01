using EP.Data.LoggerSink;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting;
using Serilog;

namespace EP.API.Infrastructure
{
    public static class LogCreator
    {
        public static Logger Create(string connectionString)
        {
            var logLevel = LogEventLevel.Warning;

            return new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", logLevel)
                .MinimumLevel.Override("System", logLevel)
                .WriteTo.MongoDb(connectionString, restrictedToMinimumLevel: logLevel)
                .CreateLogger();
        }

        private static LoggerConfiguration MongoDb(
            this LoggerSinkConfiguration loggerConfiguration,
            string connectionString,
            string collectionName = "Logs",
            ITextFormatter formatter = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            // new RenderedCompactJsonFormatter()
            formatter = formatter ?? new CompactJsonFormatter();

            return loggerConfiguration.Sink(
                new MongoDbSink(connectionString, collectionName, formatter),
                restrictedToMinimumLevel);
        }
    }
}