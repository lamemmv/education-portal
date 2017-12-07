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

        public ShortUser CreatedBy { get; set; }

        public DateTime? CreatedOnUtc { get; set; }

        public ShortUser UpdatedBy { get; set; }

        public DateTime? UpdatedOnUtc { get; set; }
    }
}
