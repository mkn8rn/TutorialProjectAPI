namespace TutorialProjectAPI.DTOs
{
    public class CreateReplyWithTempPostIdDTO
    {
        // DTO the only information a useer would need to post a reply, using tempId ass a method of 
        // handling bulk requests, so asa to sort things before adding them to teh database
        public string Body { get; set; }
        public Guid UserId { get; set; }
        public int PostTempId { get; set; }
    }

}
