using SocialMediaPostManager.Dtos;
using SocialMediaPostManager.Models.Entities;
using SocialMediaPostManager.Repositories.Implenentations;
using SocialMediaPostManager.Repositories.Interfaces;
using SocialMediaPostManager.Services.Interfaces;

namespace SocialMediaPostManager.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IReplyRepository _replyRepository;

        public CommentService()
        {
            _postRepository = new PostRepository();
            _commentRepository = new CommentRepository();
            _replyRepository = new ReplyRepository();
        }

        public Result<string> AddComment(CommentRequestModel commentRequestModel)
        {
            var post = _postRepository.GetPost(commentRequestModel.PostId);

            if (post == null)
            {
                return new Result<string>
                {
                    Data = null,
                    Message = "Post not found",
                    Status = false
                };
            }

            var comment = new Comment
            {
                CreatedBy = commentRequestModel.CreatedBy,
                PostId = commentRequestModel.PostId,
                Message = commentRequestModel.Message
            };
            _commentRepository.Persist(comment);
            return new Result<string>
            {
                Data = comment.Id.ToString(),
                Message = $"You have successfully commented on post Title: {post.Title}",
                Status = true
            };
        }

        public Result<string> EditComment(EditCommentRequestModel editCommentRequestModel)
        {
            var comment = _commentRepository.GetComment(editCommentRequestModel.Id);
            if (comment == null)
            {
                return new Result<string>
                {
                    Data = null,
                    Message = "Comment not found",
                    Status = false
                };
            }

            comment.Message = editCommentRequestModel.Message;
            _commentRepository.Update(comment);
            return new Result<string>
            {
                Data = comment.Id.ToString(),
                Message = $"You have successfully edited your comment",
                Status = true
            };
        }

        // public ICollection<CommentDto> GetAllComments()
        // {
        //     List<CommentDto> comments = [];
        //     foreach (var comment in _commentRepository.GetComments())
        //     {
        //         comments.Add(new CommentDto
        //         {
        //             Id = comment.Id,
        //             CreatedBy = comment.CreatedBy,
        //             Message = comment.Message,
        //             DateCreated = comment.DateCreated,
        //             ReplyCount = _replyRepository.GetReplyCount(comment.Id)
        //         });
        //     }
        //     return comments;
        // }

        public CommentDto? GetComment(Guid id)
        {
            var comment = _commentRepository.GetComment(id);
            if (comment != null)
            {
                return new CommentDto
                {
                    Id = comment.Id,
                    CreatedBy = comment.CreatedBy,
                    Message = comment.Message,
                    DateCreated = comment.DateCreated,
                    ReplyCount = _replyRepository.GetReplyCount(comment.Id)
                };
            }
            return null;
        }

        public ICollection<CommentDto> GetComments(Guid id)
        {
            List<CommentDto> comments = [];
            foreach (var comment in _commentRepository.GetComments(id))
            {
                comments.Add(new CommentDto
                {
                    Id = comment.Id,
                    CreatedBy = comment.CreatedBy,
                    Message = comment.Message,
                    DateCreated = comment.DateCreated,
                    ReplyCount = _replyRepository.GetReplyCount(comment.Id)
                });
            }
            return comments;
        }

        public ICollection<CommentDto> GetComments()
        {
            List<CommentDto> comments = [];
            foreach (var comment in _commentRepository.GetComments())
            {
                comments.Add(new CommentDto
                {
                    Id = comment.Id,
                    CreatedBy = comment.CreatedBy,
                    Message = comment.Message,
                    DateCreated = comment.DateCreated,
                    ReplyCount = _replyRepository.GetReplyCount(comment.Id)
                });
            }
            return comments;
        }
    }
}