using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaPostManager.Models.Entities
{
    public class Post : BaseEntity
    {
        public string? Title { get; set; }
        public required string Content { get; set; }
        public Guid SocialMediaUserId { get; set; }
    }
}