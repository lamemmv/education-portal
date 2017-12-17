using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities.Emails
{
    public class QueuedEmail : Entity
    {
        [BsonElement("priority")]
        public int Priority { get; set; }

        [BsonElement("from")]
        public string From { get; set; }

        [BsonElement("fname")]
        [BsonIgnoreIfNull]
        public string FromName { get; set; }

        [BsonElement("to")]
        public string To { get; set; }

        [BsonElement("tname")]
        [BsonIgnoreIfNull]
        public string ToName { get; set; }

        [BsonElement("replyto")]
        [BsonIgnoreIfNull]
        public string ReplyTo { get; set; }

        [BsonElement("rname")]
        [BsonIgnoreIfNull]
        public string ReplyToName { get; set; }

        [BsonElement("cc")]
        [BsonIgnoreIfNull]
        public string CC { get; set; }

        [BsonElement("bcc")]
        [BsonIgnoreIfNull]
        public string BCC { get; set; }

        [BsonElement("sub")]
        public string Subject { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("dontsendbeforedateutc")]
        [BsonIgnoreIfNull]
        public DateTime? DontSendBeforeDateUtc { get; set; }

        [BsonElement("senttries")]
        public int SentTries { get; set; }

        [BsonElement("sentonutc")]
        [BsonIgnoreIfNull]
        public DateTime? SentOnUtc { get; set; }

        [BsonElement("sendmanually")]
        public bool SendManually { get; set; }

        [BsonElement("failedreason")]
        [BsonIgnoreIfNull]
        public string FailedReason { get; set; }

        [BsonElement("emailaccountid")]
        public int EmailAccountId { get; set; }
    }
}
