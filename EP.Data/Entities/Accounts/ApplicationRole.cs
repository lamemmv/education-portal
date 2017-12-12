using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EP.Data.Entities.Accounts
{
    public class ApplicationRole : Entity
    {
        public ApplicationRole()
            : base()
        {
        }

        public ApplicationRole(string roleName)
            : base()
        {
            Name = roleName;
        }

        [BsonElement("name")]
        public virtual string Name { get; set; }

        [BsonElement("normalizedname")]
        public virtual string NormalizedName { get; set; }

        /// <summary>
        /// A random value that should change whenever a role is persisted to the store.
        /// </summary>
        [BsonElement("concurrencystamp")]
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public override string ToString()
        {
            return Name;
        }
    }
}
