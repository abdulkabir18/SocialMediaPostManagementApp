using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaPostManager.Dtos;

namespace SocialMediaPostManager.Services.Interfaces
{
    public interface IReplyService
    {
        Result<string> AddReply(CreateReplyRequestModel model);
        ICollection<ReplyDto> GetReplies(Guid id);
        ReplyDto? GetReply(Guid id);
        ICollection<ReplyDto> GetReplies();
    }
}