using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities.Logs
{
    public class ActivityLog : Entity
    {
        [BsonElement("objname")]
        public string ObjectFullName { get; set; }

        [BsonElement("oldval")]
        public string OldValue { get; set; }

        [BsonElement("newval")]
        public string NewValue { get; set; }

        [BsonElement("ip")]
        public string IP { get; set; }

        [BsonElement("alt")]
        public ShortActivityLogType ActivityLogType { get; set; }
    }
}
