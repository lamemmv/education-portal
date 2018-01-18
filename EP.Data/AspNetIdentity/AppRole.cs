using EP.Data.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace EP.Data.AspNetIdentity
{
    public class AppRole : Entity
    {
        public AppRole()
            : base()
        {
        }

        public AppRole(string roleName)
            : base()
        {
            Name = roleName;
        }

        public virtual string Name { get; set; }

        public virtual string NormalizedName { get; set; }

        /// <summary>
        /// A random value that should change whenever a role is persisted to the store.
        /// </summary>
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        [BsonIgnoreIfNull]
        public virtual List<AppClaim> Claims { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}