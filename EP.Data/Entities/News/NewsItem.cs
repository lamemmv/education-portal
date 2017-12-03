using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities.News
{
    public class NewsItem : Entity
    {
        public string Title { get; set; }

        [BsonIgnoreIfNull]
        public string ShortContent { get; set; }

        public string FullContent { get; set; }

        public bool Published { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? CreatedOnUtc { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
