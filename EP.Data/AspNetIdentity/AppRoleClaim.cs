using MongoDB.Bson.Serialization.Attributes;
using System.Security.Claims;

namespace EP.Data.AspNetIdentity
{
    public class AppRoleClaim
    {
        public AppRoleClaim()
        {
        }

        public AppRoleClaim(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        [BsonElement("type")]
        public string ClaimType { get; set; }

        [BsonElement("value")]
        public string ClaimValue { get; set; }

        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }

        public virtual void InitializeFromClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }
}
