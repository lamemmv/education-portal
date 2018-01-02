using EP.Data.Entities.Blobs;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities.News
{
    public class NewsItem : Entity
    {
        public string Title { get; set; }

        [BsonIgnoreIfNull]
        public EmbeddedBlob Blob { get; set; }

        [BsonIgnoreIfNull]
        public string Ingress { get; set; }

        public string Content { get; set; }   

        public bool Published { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? PublishedDate { get; set; }
    }
}
