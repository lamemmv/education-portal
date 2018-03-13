using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities
{
    public abstract class Entity : IEntity
    {
        [BsonElement(Order = 1)]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
