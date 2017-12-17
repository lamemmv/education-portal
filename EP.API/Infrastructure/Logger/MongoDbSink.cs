using MongoDB.Bson;
using MongoDB.Driver;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting;
using System.IO;

namespace EP.API.Infrastructure.Logger
{
    public sealed class MongoDbSink : ILogEventSink
    {
        public const string DefaultCollectionName = "Logs";

        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly ITextFormatter _formatter;

        public MongoDbSink(
            string connectionString,
            string collectionName = DefaultCollectionName)
        {
            _collection = GetMongoCollection(connectionString, collectionName);
            _formatter = GetJsonFormatter();
        }

        public void Emit(LogEvent logEvent)
        {
            BsonDocument bsonDocument = GenerateBsonDocument(logEvent);
            _collection.InsertOne(bsonDocument);
        }

        private static IMongoCollection<BsonDocument> GetMongoCollection(
            string connectionString,
            string collectionName)
        {
            MongoUrl url = new MongoUrl(connectionString);
            IMongoClient client = new MongoClient(url);
            IMongoDatabase database = client.GetDatabase(url.DatabaseName);

            return database.GetCollection<BsonDocument>(collectionName);
        }

        private static ITextFormatter GetJsonFormatter()
        {
            return new RenderedCompactJsonFormatter();
        }

        private BsonDocument GenerateBsonDocument(LogEvent logEvent)
        {
            BsonDocument bsonDocument;
            TextWriter writer = new StringWriter();
            _formatter.Format(logEvent, writer);

            BsonDocument.TryParse(writer.ToString(), out bsonDocument);

            return bsonDocument;
        }
    }
}