using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities.Blobs
{
    public class Blob : Entity
    {
        [BsonElement("name")]
        public string FileName { get; set; }

        [BsonElement("ext")]
        public string FileExtension { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("type")]
        public string ContentType { get; set; }

        [BsonElement("path")]
        public string PhysicalPath { get; set; }
    }
}
