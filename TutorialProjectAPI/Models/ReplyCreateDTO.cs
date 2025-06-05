using System;

namespace TutorialProjectAPI.Models
{
    public class ReplyCreateDTO
    {
        public string Body { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
    }
}