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

        public ShortUser CreatedBy { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        public ShortUser UpdatedBy { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }
    }
}
