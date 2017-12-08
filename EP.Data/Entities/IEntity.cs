using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities
{
    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("cby")]
        ShortUser CreatedBy { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("con")]
        DateTime? CreatedOnUtc { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("uby")]
        ShortUser UpdatedBy { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("uon")]
        DateTime? UpdatedOnUtc { get; set; }
    }
}
