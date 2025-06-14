using System;
using System.ComponentModel.DataAnnotations;

namespace TutorialProjectAPI.Models
{
    public class ImageDB : IIdentifiableDB
    {
        public Guid Id { get; set; }

        [MaxLength(136000)] // 100kb limit
        public string Base64Image { get; set; }
    }
}