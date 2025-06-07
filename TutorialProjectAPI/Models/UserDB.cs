using System;

namespace TutorialProjectAPI.Models
{
    public class UserDB : IIdentifiableDB
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
