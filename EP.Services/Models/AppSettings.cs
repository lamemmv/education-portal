using Microsoft.AspNetCore.Identity;

namespace EP.Services.Models
{
    public sealed class AppSettings
    {
        public string HostUrl { get; set; }

        public BlobFolders BlobFolders { get; set; }

        public PasswordOptions PasswordPolicies { get; set; }

        public LockoutPolicies LockoutPolicies { get; set; }

        public SignInOptions SignInPolicies { get; set; }
    }
}
