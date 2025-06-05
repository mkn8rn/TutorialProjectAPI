using System;

namespace TutorialProjectAPI.Models
{
    public class ReplyDB : IIdentifiableDB
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public Guid PostId { get; set; }
        public PostDB Post { get; set; }
        public Guid UserId { get; set; }
        public UserDB User { get; set; }
    }
}