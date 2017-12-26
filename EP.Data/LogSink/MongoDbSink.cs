using EP.Data.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using System.IO;

namespace EP.Data.LogSink
{
    public sealed class MongoDbSink : ILogEventSink
    {
        private readonly string _connectionString;
        private readonly string _collectionName;
        private readonly ITextFormatter _formatter;

        public MongoDbSink(string connectionString, string collectionName, ITextFormatter formatter)
        {
            _connectionString = connectionString;
            _collectionName = collectionName;
            _formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            BsonDocument bsonDocument = GenerateBsonDocument(logEvent, _formatter);

            if (bsonDocument != null)
            {
                GetMongoCollection(_connectionString, _collectionName)
                    .InsertOne(bsonDocument);
            }
        }

        private static BsonDocument GenerateBsonDocument(LogEvent logEvent, ITextFormatter formatter)
        {
            TextWriter writer = new StringWriter();
            formatter.Format(logEvent, writer);

            BsonDocument.TryParse(writer.ToString(), out BsonDocument bsonDocument);

            return bsonDocument;
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