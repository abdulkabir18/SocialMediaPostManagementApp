using SocialMediaPostManager.Dtos;

namespace SocialMediaPostManager.Services.Interfaces
{
    public interface ICommentService
    {
        Result<string> AddComment(CommentRequestModel commentRequestModel);
        Result<string> EditComment(EditCommentRequestModel editCommentRequestModel);
        ICollection<CommentDto> GetComments(Guid id);
        ICollection<CommentDto> GetComments();
        ICollection<CommentDto> GetAllComments();
    }
}