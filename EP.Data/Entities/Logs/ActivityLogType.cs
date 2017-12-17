using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities.Logs
{
    public class ActivityLogType : Entity
    {
        public string SystemKeyword { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }
    }
}
