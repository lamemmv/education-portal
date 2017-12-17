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
            Type = claimType;
            Value = claimValue;
        }

        public string Type { get; set; }

        public string Value { get; set; }

        public virtual Claim ToClaim()
        {
            return new Claim(Type, Value);
        }

        public virtual void InitializeFromClaim(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
        }
    }
}
