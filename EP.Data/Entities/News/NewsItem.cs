using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities.News
{
    public class NewsItem : Entity
    {
        public string Title { get; set; }

        [BsonIgnoreIfNull]
        public string Ingress { get; set; }

        public string Content { get; set; }

        [BsonIgnoreIfNull]
        public string Image { get; set; }

        public bool Published { get; set; }

        [BsonIgnoreIfNull]
        [BsonDateTimeOptions(DateOnly = true, Kind = DateTimeKind.Local)]
        public DateTime? PublishedDate { get; set; }
    }
}
