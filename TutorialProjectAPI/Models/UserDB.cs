namespace TutorialProjectAPI.Models
{
    public class UserDB : IIdentifiableDB
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public Guid? ProfileImageId { get; set; }
        public ImageDB ProfileImage { get; set; }
    }
}
