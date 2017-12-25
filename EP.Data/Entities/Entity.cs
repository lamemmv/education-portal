using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities
{
    public abstract class Entity : IEntity
    {
        [BsonElement(Order = 1)]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonIgnoreIfNull]
        public EmbeddedUser Creator { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? CreatedOn { get; set; }

        [BsonIgnoreIfNull]
        public EmbeddedUser Updater { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? UpdatedOn { get; set; }
    }
}
