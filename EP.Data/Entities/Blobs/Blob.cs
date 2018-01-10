using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace EP.Data.Entities.Blobs
{
    public class Blob : Entity
    {
        public string Name { get; set; }

        public string RandomName { get; set; }

        [BsonIgnoreIfNull]
        public string FileExtension { get; set; }

        [BsonIgnoreIfNull]
        public string ContentType { get; set; }

        [BsonIgnoreIfNull]
        public string VirtualPath { get; set; }

        [BsonIgnoreIfNull]
        public string PhysicalPath { get; set; }

        [BsonIgnoreIfNull]
        public string Parent { get; set; }

        [BsonIgnoreIfNull]
        public IEnumerable<BlobAncestor> Ancestors { get; set; }
    }
}
