using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaPostManager.Dtos
{
    public class ReplyDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public Guid CommentId { get; set; }
        public Guid MediaUserId { get; set; }
        public required string Message { get; set; }
    }

    public class CreateReplyRequestModel
    {
        public string? CreatedBy { get; set; }
        public Guid CommentId { get; set; }
        public required Guid MediaUserId { get; set; }
        public required string Message { get; set; }
    }

    public class DeleteReplyRequestModel
    {
        public Guid Id { get; set; }
    }
}