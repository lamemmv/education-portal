using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities.News
{
    public class NewsItem : Entity
    {
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("ingress")]
        public string Ingress { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("pub")]
        public bool Published { get; set; }

        [BsonDateTimeOptions(DateOnly = true, Kind = DateTimeKind.Local)]
        [BsonElement("pubdate")]
        public DateTime? PublishedDate { get; set; }
    }
}
