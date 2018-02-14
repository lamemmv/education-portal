using System.Security.Claims;

namespace EP.Data.AspNetIdentity
{
    public class AppClaim
    {
        public AppClaim()
        {
        }

        public AppClaim(string claimType, string claimValue)
        {
            Type = claimType;
            Value = claimValue;
        }

        public string Type { get; set; }

        public string Value { get; set; }

        public virtual Claim ToClaim()
            => new Claim(Type, Value);

        public virtual void InitializeFromClaim(Claim claim)
        {
            Type = claim.Type;
            Value = claim.Value;
        }
    }
}
