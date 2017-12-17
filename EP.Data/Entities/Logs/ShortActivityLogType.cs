using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities.Logs
{
    public class ShortActivityLogType
    {
        public string Id { get; set; }

        public string SystemKeyword { get; set; }

        public string Name { get; set; }
    }
}
