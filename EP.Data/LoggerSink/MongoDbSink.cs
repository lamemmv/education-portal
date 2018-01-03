using EP.Data.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EP.Data.LoggerSink
{
    public sealed class MongoDbSink : PeriodicBatchingSink
    {
        public const string DefaultCollectionName = "Logs";
        public const int DefaultBatchPostingLimit = 50;
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(2);

        private readonly string _connectionString;
        private readonly string _collectionName;
        private readonly ITextFormatter _formatter;

        public MongoDbSink(
            ITextFormatter formatter,
            string connectionString,
            string collectionName = DefaultCollectionName,            
            int batchPostingLimit = DefaultBatchPostingLimit,
            TimeSpan? period = null) : base(batchPostingLimit, period ?? DefaultPeriod)
        {
            _formatter = formatter;
            _connectionString = connectionString;
            _collectionName = collectionName;
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            if (events == null || !events.Any())
            {
                var documents = GenerateBsonDocuments(events, _formatter);

                await GetMongoCollection(_connectionString, _collectionName)
                    .InsertManyAsync(documents);
            }
        }

        private static IEnumerable<BsonDocument> GenerateBsonDocuments(
            IEnumerable<LogEvent> events,
            ITextFormatter formatter)
        {
            TextWriter writer;

            foreach (var logEvent in events)
            {
                writer = new StringWriter();
                formatter.Format(logEvent, writer);

                BsonDocument.TryParse(writer.ToString(), out BsonDocument bsonDocument);

                yield return bsonDocument;
            }
        }

        private static IMongoCollection<BsonDocument> GetMongoCollection(
            string connectionString,
            string collectionName)
        {
            return MongoDbHelper
                .GetMongoDatabase(connectionString)
                .GetCollection<BsonDocument>(collectionName);
        }
    }
}