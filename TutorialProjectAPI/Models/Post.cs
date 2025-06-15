using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TutorialProjectAPI.Models
{
    public class Post
    {
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        public Guid UserId { get; set; }
        public UserDB User { get; set; }

        public List<Reply> Replies { get; set; }

        public int? ImageId { get; set; }
        public ImageDB Image { get; set; }

    }


}
