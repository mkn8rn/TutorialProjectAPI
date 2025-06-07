namespace TutorialProjectAPI.DTOs
{
    public class ReplyDto
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public string Username { get; set; } // From User
        public int PostId { get; set; }
    }

}
