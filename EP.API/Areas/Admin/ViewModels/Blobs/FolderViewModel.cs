using System.ComponentModel.DataAnnotations;

namespace EP.API.Areas.Admin.ViewModels.Blobs
{
    public sealed class FolderViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Parent { get; set; }
    }
}