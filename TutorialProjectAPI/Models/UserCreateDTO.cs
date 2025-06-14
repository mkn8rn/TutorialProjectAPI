public class UserCreateDTO
{
    public string Username { get; set; }
    public string? ProfileImageBase64 { get; set; }
    public Guid? ProfileImageId { get; set; }
}