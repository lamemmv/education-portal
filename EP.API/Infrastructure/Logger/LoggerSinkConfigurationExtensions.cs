//using Serilog;
//using Serilog.Configuration;
//using Serilog.Events;

//namespace EP.API.Infrastructure.Logger
//{
//    public static class LoggerSinkConfigurationExtensions
//    {
//        public static LoggerConfiguration MongoDB(
//            this LoggerSinkConfiguration loggerConfiguration,
//            string connectionString,
//            string databaseName,
//            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
//        {
//            return loggerConfiguration.Sink(
//                new MongoDbSink(connectionString, databaseName),
//                restrictedToMinimumLevel);
//        }
//    }
//}
