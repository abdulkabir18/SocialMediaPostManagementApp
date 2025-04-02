using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaPostManager.Dtos
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public bool IsDelete { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public string? Title { get; set; }
        public required string Content { get; set; }
        public Guid SocialMediaUserId { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }

    public class EditPostRequestModel
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public required string Content { get; set; }
    }

    public class CreatePostRequestModel
    {
        public required string CreatedBy { get; set; }
        public string? Title { get; set; }
        public required string Content { get; set; }
        public Guid SocialMediaUserId { get; set; }
    }
}