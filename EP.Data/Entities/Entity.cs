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

        [BsonElement(Order = 0)]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public ObjectId ObjectId => ObjectId.Parse(Id);

        [BsonIgnoreIfNull]
        public ShortUser CreatedBy { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? CreatedOnUtc { get; set; }

        [BsonIgnoreIfNull]
        public ShortUser UpdatedBy { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
