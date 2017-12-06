//using EP.Data.Entities.Logs;
//using EP.Data.Repositories;
//using MongoDB.Driver;
//using Serilog.Core;
//using Serilog.Events;
//using System;

//namespace EP.API.Infrastructure.Logger
//{
//    public sealed class MongoDbSink : ILogEventSink
//    {
//        private const string DefaultCollectionName = "Logs";
//        private readonly IRepository<Log> _logs;

//        public MongoDbSink(string connectionString, string databaseName)
//        {
//            _logs = GetLogRepository(connectionString, databaseName);
//        }

//        public void Emit(LogEvent logEvent)
//        {
//            throw new NotImplementedException();
//        }

//        private static IRepository<Log> GetLogRepository(string connectionString, string databaseName)
//        {
//            IMongoClient client = new MongoClient(connectionString);
//            IMongoDatabase database = client.GetDatabase(databaseName);
//            IMongoCollection<Log> logs = database.GetCollection<Log>(DefaultCollectionName);

//            return new MongoRepository<Log>(logs);
//        }

//        private static Log GeLogEntity(LogEvent logEvent)
//        {
//            return new Log
//            {
//                Timestamp = logEvent.Timestamp.ToUniversalTime().DateTime,
//                Level = logEvent.Level.ToString(),
//                Message = logEvent.RenderMessage(),
//                MessageTemplate = logEvent.MessageTemplate.Text,
//                Exception = logEvent.Exception.ToString(),
//                Properties = logEvent.Properties
//            };
//        }
//    }
//}
