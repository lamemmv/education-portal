using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities
{
    public interface IEntity
    {
        [BsonId]
        string Id { get; set; }

        [BsonIgnoreIfNull]
        EmbeddedUser Creator { get; set; }

        [BsonIgnoreIfNull]
        DateTime? CreatedOn { get; set; }

        [BsonIgnoreIfNull]
        EmbeddedUser Updater { get; set; }

        [BsonIgnoreIfNull]
        DateTime? UpdatedOn { get; set; }
    }
}
