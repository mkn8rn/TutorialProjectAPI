using System;

namespace TutorialProjectAPI.Models
{
    public interface IIdentifiableDB
    {
        Guid Id { get; set; }
    }
}
