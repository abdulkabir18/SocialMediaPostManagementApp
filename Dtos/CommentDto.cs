namespace SocialMediaPostManager.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public required string Message { get; set; }
        public int ReplyCount { get; set; }
    }

    public class EditCommentRequestModel
    {
        public Guid Id { get; set; }
        public required string Message { get; set; }
    }

    public class DeleteCommentRequestModel
    {
        public Guid Id { get; set; }
    }

    public class CommentRequestModel
    {
        public string? CreatedBy { get; set; }
        public Guid PostId { get; set; }
        public required string Message { get; set; }
    }
}