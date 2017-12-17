using System.ComponentModel.DataAnnotations;

namespace EP.API.Areas.Admin.ViewModels.Emails
{
    public sealed class EmailAccountViewModel
    {
        [Required]
        public string Email { get; set; }

        public string DisplayName { get; set; }

        [Required]
        public string Host { get; set; }

        [Range(1, ushort.MaxValue)]
        public int Port { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool EnableSsl { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public bool IsDefault { get; set; }
    }
}
