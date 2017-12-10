using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities
{
    public abstract class Entity : IEntity
    {
        public Entity()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }

        [BsonElement(Order = 1)]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("cor")]
        public ShortUser Creator { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("con")]
        public DateTime? CreatedOnUtc { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("uer")]
        public ShortUser Updater { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("uon")]
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
