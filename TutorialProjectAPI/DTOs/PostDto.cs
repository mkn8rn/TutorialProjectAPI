namespace TutorialProjectAPI.DTOs
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Username { get; set; } // From User
    }

}
