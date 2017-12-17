using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities
{
    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }

        [BsonIgnoreIfNull]
        ShortUser Creator { get; set; }

        [BsonIgnoreIfNull]
        DateTime? CreatedOn { get; set; }

        [BsonIgnoreIfNull]
        ShortUser Updater { get; set; }

        [BsonIgnoreIfNull]
        DateTime? UpdatedOn { get; set; }
    }
}
