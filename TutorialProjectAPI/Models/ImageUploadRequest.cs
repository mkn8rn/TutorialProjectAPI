using System.ComponentModel.DataAnnotations;

namespace TutorialProjectAPI.Models
{
    public class ImageUploadRequest
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }

}
