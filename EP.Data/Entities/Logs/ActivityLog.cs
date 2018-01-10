using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities.Logs
{
    public class ActivityLog : Entity
    {
        public string EntityName { get; set; }

        public string LogValue { get; set; }

        public string IP { get; set; }

        public EmbeddedActivityLogType ActivityLogType { get; set; }
    }
}
