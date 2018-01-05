using System.ComponentModel.DataAnnotations;

namespace EP.API.Areas.Admin.ViewModels.Blobs
{
    public sealed class DirectoryViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}