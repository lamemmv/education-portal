using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities.Logs
{
    public class Log : Entity
    {
        [BsonElement("@t")]
        public DateTime Timestamp { get; set; }

        [BsonElement("@m")]
        public string Message { get; set; }

        [BsonElement("@mt")]
        public string MessageTemplate { get; set; }

        [BsonElement("@l")]
        public string Level { get; set; }

        [BsonElement("@x")]
        public string Exception { get; set; }

        [BsonElement("@i")]
        public string EventId { get; set; }
    }
}