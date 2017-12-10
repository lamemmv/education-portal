using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities
{
    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("cor")]
        ShortUser Creator { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("con")]
        DateTime? CreatedOnUtc { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("uer")]
        ShortUser Updater { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("uon")]
        DateTime? UpdatedOnUtc { get; set; }
    }
}
