using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TutorialProjectAPI.Models
{
    public class Reply
    {
        [Key]
        public int CommentId { get; set; }
        public string Text { get; set; }

        public Guid UserId { get; set; }
        public virtual UserDB User { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public Reply() { }
    }

}
