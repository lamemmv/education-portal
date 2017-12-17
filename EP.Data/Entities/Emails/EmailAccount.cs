using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities.Emails
{
    public class EmailAccount : Entity
    {
        [BsonElement("email")]
        public string Email { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("displayname")]
        public string DisplayName { get; set; }

        [BsonElement("host")]
        public string Host { get; set; }

        [BsonElement("port")]
        public int Port { get; set; }

        [BsonElement("uname")]
        public string UserName { get; set; }

        [BsonElement("pwd")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value that controls whether the SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection.
        /// </summary>
        [BsonElement("ssl")]
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or sets a value that controls whether the default system credentials of the application are sent with requests.
        /// </summary>
        [BsonElement("defaultcredentials")]
        public bool UseDefaultCredentials { get; set; }

        [BsonElement("isdefault")]
        public bool IsDefault { get; set; }
    }
}
