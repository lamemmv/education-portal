using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EP.API.Areas.Admin.ViewModels.Blobs
{
    public sealed class BlobViewModel
    {
        public string Name { get; set; }

        public IFormFile[] Files { get; set; }

        [Required]
        public string Parent { get; set; }
       
        public bool IsValidDirectory
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Name);
            }
        }

        public bool IsValidFile
        {
            get
            {
                return Files != null && Files.Length > 0;
            }
        }
    }
}