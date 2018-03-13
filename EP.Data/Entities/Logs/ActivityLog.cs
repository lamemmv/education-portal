using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities.Logs
{
    public class ActivityLog : Entity
    {
        public string EntityName { get; set; }

        public string LogValue { get; set; }

        public string IP { get; set; }

        public DateTime CreatedOn { get; set; }

        public EmbeddedUser CreatedBy { get; set; }

        public EmbeddedActivityLogType ActivityLogType { get; set; }
    }
}
