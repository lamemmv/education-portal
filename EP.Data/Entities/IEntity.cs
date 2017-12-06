using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities
{
    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }

        [BsonIgnore]
        ObjectId ObjectId { get; }

        [BsonIgnoreIfNull]
        ShortUser CreatedBy { get; set; }

        [BsonIgnoreIfNull]
        DateTime? CreatedOnUtc { get; set; }

        [BsonIgnoreIfNull]
        ShortUser UpdatedBy { get; set; }

        [BsonIgnoreIfNull]
        DateTime? UpdatedOnUtc { get; set; }
    }
}
