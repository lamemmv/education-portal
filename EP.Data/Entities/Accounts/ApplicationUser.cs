using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace EP.Data.Entities.Accounts
{
    public class ApplicationUser : Entity
    {
        public ApplicationUser()
            : base()
        {
        }

        public ApplicationUser(string userName)
            : base()
        {
            UserName = userName;
        }

        [BsonElement("username")]
        public virtual string UserName { get; set; }

        [BsonElement("normalizedusername")]
        public virtual string NormalizedUserName { get; set; }

        [BsonElement("email")]
        public virtual string Email { get; set; }

        [BsonElement("normalizedemail")]
        public virtual string NormalizedEmail { get; set; }

        [BsonElement("emailconfirmed")]
        public virtual bool EmailConfirmed { get; set; }

        [BsonElement("passwordhash")]
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed).
        /// </summary>
        [BsonElement("securitystamp")]
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// A random value that must change whenever a user is persisted to the store.
        /// </summary>
        [BsonElement("concurrencystamp")]
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        [BsonElement("phonenumber")]
        public virtual string PhoneNumber { get; set; }

        [BsonElement("phonenumberconfirmed")]
        public virtual bool PhoneNumberConfirmed { get; set; }

        [BsonElement("twofactorenabled")]
        public virtual bool TwoFactorEnabled { get; set; }

        [BsonElement("lockoutend")]
        [BsonIgnoreIfNull]
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        [BsonElement("lockoutenabled")]
        public virtual bool LockoutEnabled { get; set; }

        [BsonElement("accessfailedcount")]
        public virtual int AccessFailedCount { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("roles")]
        public virtual List<string> Roles { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("logins")]
        public virtual List<ApplicationUserLogin> Logins { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("claims")]
        public virtual List<ApplicationUserClaim> Claims { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("tokens")]
        public virtual IList<ApplicationUserToken> Tokens { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }
}
