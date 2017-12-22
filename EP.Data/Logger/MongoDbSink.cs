using MongoDB.Bson;
using MongoDB.Driver;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting;
using System.IO;

namespace EP.Data.Logger
{
    public sealed class MongoDbSink : ILogEventSink
    {
        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly ITextFormatter _formatter;

        public MongoDbSink(IMongoCollection<BsonDocument> collection)
        {
            _collection = collection;
            _formatter = GetJsonFormatter();
        }

        public void Emit(LogEvent logEvent)
        {
            BsonDocument bsonDocument = GenerateBsonDocument(logEvent);

            if (bsonDocument != null)
            {
                _collection.InsertOne(bsonDocument);
            }
        }

        private static ITextFormatter GetJsonFormatter()
        {
            // RenderedCompactJsonFormatter().
            return new CompactJsonFormatter();
        }

        private BsonDocument GenerateBsonDocument(LogEvent logEvent)
        {
            TextWriter writer = new StringWriter();
            _formatter.Format(logEvent, writer);

            BsonDocument bsonDocument;
            BsonDocument.TryParse(writer.ToString(), out bsonDocument);

            return bsonDocument;
        }
    }
}