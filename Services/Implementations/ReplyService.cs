using SocialMediaPostManager.Dtos;
using SocialMediaPostManager.Models.Entities;
using SocialMediaPostManager.Repositories.Implenentations;
using SocialMediaPostManager.Repositories.Interfaces;
using SocialMediaPostManager.Services.Interfaces;

namespace SocialMediaPostManager.Services.Implementations
{
    public class ReplyService : IReplyService
    {
        private readonly IReplyRepository _replyRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ISocialMediaUserRepository _socialMediaUserRepository;

        public ReplyService()
        {
            _replyRepository = new ReplyRepository();
            _commentRepository = new CommentRepository();
            _socialMediaUserRepository = new SocialMediaUserRepository();
        }

        public Result<string> AddReply(CreateReplyRequestModel model)
        {
            bool checkComment = _commentRepository.CheckComment(model.CommentId);
            if (!checkComment)
            {
                return new Result<string>
                {
                    Data = null,
                    Message = "Comment not found",
                    Status = false
                };
            }

            var reply = new Reply
            {
                Message = model.Message,
                CreatedBy = model.CreatedBy,
                CommentId = model.CommentId,
                MediaUserId = model.MediaUserId
            };

            _replyRepository.Persist(reply);

            return new Result<string>
            {
                Data = reply.Id.ToString(),
                Message = "Reply added successfully",
                Status = true
            };
        }

        public ICollection<ReplyDto> GetReplies(Guid id)
        {
            List<ReplyDto> replies = [];
            foreach (var reply in _replyRepository.GetReplies(id))
            {
                var _reply = new ReplyDto
                {
                    Message = reply.Message,
                    CommentId = reply.CommentId,
                    CreatedBy = reply.CreatedBy,
                    DateCreated = reply.DateCreated,
                    Id = reply.Id,
                    MediaUserId = reply.MediaUserId
                };
                replies.Add(_reply);
            }
            return replies;
        }

        public ICollection<ReplyDto> GetReplies()
        {
            List<ReplyDto> replies = [];
            foreach (var reply in _replyRepository.GetReplies())
            {
                var _reply = new ReplyDto
                {
                    Message = reply.Message,
                    CommentId = reply.CommentId,
                    CreatedBy = reply.CreatedBy,
                    DateCreated = reply.DateCreated,
                    Id = reply.Id,
                    MediaUserId = reply.MediaUserId
                };
                replies.Add(_reply);
            }
            return replies;
        }

        public ReplyDto? GetReply(Guid id)
        {
            var reply = _replyRepository.GetReply(id);
            if (reply != null)
            {
                return new ReplyDto
                {
                    Message = reply.Message,
                    CommentId = reply.CommentId,
                    CreatedBy = reply.CreatedBy,
                    DateCreated = reply.DateCreated,
                    Id = reply.Id,
                    MediaUserId = reply.MediaUserId
                };
            }
            return null;
        }
    }
}