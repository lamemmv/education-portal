using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities.Blobs
{
    public class Blob : Entity
    {
        public string FileName { get; set; }

        [BsonIgnoreIfNull]
        public int? Width { get; set; }

        [BsonIgnoreIfNull]
        public int? Height { get; set; }

        [BsonIgnoreIfNull]
        public string ContentType { get; set; }

        public string PhysicalPath { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? CreatedOnUtc { get; set; }
    }
}
