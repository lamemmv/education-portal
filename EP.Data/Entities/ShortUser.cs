using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities
{
    public class ShortUser
    {
        [BsonIgnoreIfNull]
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("uname")]
        public string UserName { get; set; }
    }
}
