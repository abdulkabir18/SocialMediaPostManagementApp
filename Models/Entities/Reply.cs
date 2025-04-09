using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaPostManager.Models.Entities
{
    public class Reply : BaseEntity
    {
        public Guid CommentId { get; set; }
        public required Guid MediaUserId { get; set; }
        public required string Message { get; set; }
    }
}