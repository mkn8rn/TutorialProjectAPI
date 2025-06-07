namespace TutorialProjectAPI.Models
{
    public class UserDB : IIdentifiableDB
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
    }
}
