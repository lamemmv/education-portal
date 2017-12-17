using MongoDB.Bson.Serialization.Attributes;

namespace EP.Data.AspNetIdentity
{
    public class AppUserLogin
    {
        public AppUserLogin()
        {
        }

        public AppUserLogin(string loginProvider, string providerKey, string providerDisplayName)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            ProviderDisplayName = providerDisplayName;
        }

        /// <summary>
        /// Gets or sets the login provider for the login (e.g. facebook, google).
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the unique provider identifier for this login.
        /// </summary>
        public virtual string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the friendly name used in a UI for this login.
        /// </summary>
        public virtual string ProviderDisplayName { get; set; }
    }
}
