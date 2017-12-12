using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities.Accounts
{
    public class ApplicationUserToken
    {
        public ApplicationUserToken()
        {
        }

        public ApplicationUserToken(string loginProvider, string name, string value)
        {
            LoginProvider = loginProvider;
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the LoginProvider this token is from.
        /// </summary>
        [BsonElement("loginprovider")]
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the name of the token.
        /// </summary>
        [BsonElement("name")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the token value.
        /// </summary>
        [BsonElement("value")]
        public virtual string Value { get; set; }
    }
}
