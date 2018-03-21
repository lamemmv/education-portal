namespace EP.Services.Models
{
    public sealed class LockoutPolicies
    {
        public bool AllowedForNewUsers { get; set; }

        public int MaxFailedAccessAttempts { get; set; }

        public int DefaultLockoutInMinutes { get; set; }
    }
}