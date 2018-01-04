using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities.Blobs
{
    public class Blob : Entity
    {
        public string Name { get; set; }

        [BsonIgnoreIfNull]
        public string FileExtension { get; set; }

        [BsonIgnoreIfNull]
        public string ContentType { get; set; }

        [BsonIgnoreIfNull]
        public string VirtualPath { get; set; }

        public string PhysicalPath { get; set; }

        [BsonIgnoreIfNull]
        public string Parent { get; set; }
    }
}
