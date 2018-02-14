using EP.Data.LoggerSink;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog;

namespace EP.API.Infrastructure
{
    public static class LogCreator
    {
        public static Logger Create(string connectionString)
        {
            var logLevel = LogEventLevel.Warning;

            return new LoggerConfiguration()
                .MinimumLevel.Override("System", logLevel)
                .MinimumLevel.Override("Microsoft", logLevel)
                .MinimumLevel.Override("IdentityServer", logLevel)
                .WriteTo.MongoDb(connectionString, logLevel)
                .CreateLogger();
        }

        private static LoggerConfiguration MongoDb(
            this LoggerSinkConfiguration loggerConfiguration,
            string connectionString,
            LogEventLevel restrictedToMinimumLevel)
            => loggerConfiguration.Sink(
                new MongoDbSink(new CompactJsonFormatter(), connectionString),
                restrictedToMinimumLevel);
    }
}