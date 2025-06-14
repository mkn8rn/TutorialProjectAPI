using System;
using System.Collections.Generic;

namespace TutorialProjectAPI.Models
{
    public class PostWithRepliesDTO
    {
        public string Body { get; set; }
        public Guid UserId { get; set; }
        public string? ImageBase64 { get; set; }
        public Guid? ImageId { get; set; }
        public List<ReplyDTO> Replies { get; set; }
    }

    public class ReplyDTO
    {
        public string Body { get; set; }
        public Guid UserId { get; set; }
    }
}