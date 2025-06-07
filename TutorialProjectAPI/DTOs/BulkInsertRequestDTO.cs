namespace TutorialProjectAPI.DTOs
{
    public class BulkInsertRequestDTO
    {
        // ChatGPT suggested idea to receive list of replies and posts all at once.
        public List<CreatePostWithTempIdDTO> Posts { get; set; }
        public List<CreateReplyWithTempPostIdDTO> Replies { get; set; }
    }
}
