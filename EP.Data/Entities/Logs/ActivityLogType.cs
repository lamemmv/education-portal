using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities.Logs
{
    public class ActivityLogType : Entity
    {
        [BsonElement("syskey")]
        public string SystemKeyword { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("enabled")]
        public bool Enabled { get; set; }
    }
}
