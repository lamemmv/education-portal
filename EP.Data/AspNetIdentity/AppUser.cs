using EP.Data.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace EP.Data.AspNetIdentity
{
    public class AppUser : Entity
    {
        public AppUser()
            : base()
        {
        }

        public AppUser(string userName)
            : base()
        {
            UserName = userName;
        }

        public virtual string UserName { get; set; }

        public virtual string NormalizedUserName { get; set; }

        public virtual string Email { get; set; }

        public virtual string NormalizedEmail { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed).
        /// </summary>
        [BsonIgnoreIfNull]
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// A random value that must change whenever a user is persisted to the store.
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        [BsonIgnoreIfNull]
        public virtual string PhoneNumber { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        public virtual bool TwoFactorEnabled { get; set; }

        [BsonIgnoreIfNull]
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual int AccessFailedCount { get; set; }

        [BsonIgnoreIfNull]
        public virtual List<string> Roles { get; set; }

        [BsonIgnoreIfNull]
        public virtual List<AppUserLogin> Logins { get; set; }

        [BsonIgnoreIfNull]
        public virtual List<AppClaim> Claims { get; set; }

        [BsonIgnoreIfNull]
        public virtual IList<AppUserToken> Tokens { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }
}
