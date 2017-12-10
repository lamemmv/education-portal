using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities.Logs
{
    public class ShortActivityLogType
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("syskey")]
        public string SystemKeyword { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}
