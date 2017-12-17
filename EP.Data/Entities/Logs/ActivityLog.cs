using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities.Logs
{
    public class ActivityLog : Entity
    {
        public string ObjectFullName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string IP { get; set; }

        public ShortActivityLogType ActivityLogType { get; set; }
    }
}
