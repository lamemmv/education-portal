using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities.Emails
{
    public class QueuedEmail : Entity
    {
        public int Priority { get; set; }

        public string From { get; set; }

        [BsonIgnoreIfNull]
        public string FromName { get; set; }

        public string To { get; set; }

        [BsonIgnoreIfNull]
        public string ToName { get; set; }

        [BsonIgnoreIfNull]
        public string ReplyTo { get; set; }

        [BsonIgnoreIfNull]
        public string ReplyToName { get; set; }

        [BsonIgnoreIfNull]
        public string CC { get; set; }

        [BsonIgnoreIfNull]
        public string BCC { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? DontSendBeforeDate { get; set; }

        public int SentTries { get; set; }

        [BsonIgnoreIfNull]
        public DateTime? SentOn { get; set; }

        public bool SendManually { get; set; }

        [BsonIgnoreIfNull]
        public string FailedReason { get; set; }

        public int EmailAccountId { get; set; }

        public EmailAccount EmailAccount { get; set; }
    }
}
