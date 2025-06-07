using System.ComponentModel.DataAnnotations;

namespace TutorialProjectAPI.Models
{
    public class ReplyDB : IIdentifiableDB
    {
        public Guid Id { get; set; }

        [Required, MaxLength(4000)]
        public string Body { get; set; } = string.Empty;

        /* FK → User */
        public Guid UserId { get; set; }
        public UserDB? User { get; set; }       // <- add this

        /* FK → Post */
        public Guid PostId { get; set; }
        public PostDB? Post { get; set; }       // <- add this
    }
}
