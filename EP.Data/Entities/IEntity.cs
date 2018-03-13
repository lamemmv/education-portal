using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities
{
    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }
    }
}
