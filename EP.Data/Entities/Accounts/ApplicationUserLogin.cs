using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.Entities.Accounts
{
    public class ApplicationUserLogin
    {
        public ApplicationUserLogin()
        {
        }

        public ApplicationUserLogin(string loginProvider, string providerKey, string providerDisplayName)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            ProviderDisplayName = providerDisplayName;
        }

        /// <summary>
        /// Gets or sets the login provider for the login (e.g. facebook, google).
        /// </summary>
        [BsonElement("loginprovider")]
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the unique provider identifier for this login.
        /// </summary>
        [BsonElement("providerkey")]
        public virtual string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the friendly name used in a UI for this login.
        /// </summary>
        [BsonElement("providerdisplayname")]
        public virtual string ProviderDisplayName { get; set; }
    }
}
