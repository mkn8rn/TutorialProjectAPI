using System;
using System.Collections.Generic;

namespace TutorialProjectAPI.Models
{
    public class PostDB : IIdentifiableDB
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public Guid UserId { get; set; }
        public UserDB User { get; set; }
        public ICollection<ReplyDB> Replies { get; set; }

        public Guid? ImageId { get; set; }
        public ImageDB Image { get; set; }
    }
}