namespace TutorialProjectAPI.DTOs
{
    public class CreatePostWithTempIdDTO
    {
        //Temp Id lets me assign a disposable ID to keep things in order befre sorting them properly into
        // the database
        public int TempId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public Guid UserId { get; set; }
    }
}
