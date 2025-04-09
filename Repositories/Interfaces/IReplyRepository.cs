using SocialMediaPostManager.Models.Entities;

namespace SocialMediaPostManager.Repositories.Interfaces
{
    public interface IReplyRepository
    {
        void Persist(Reply reply);
        Reply? GetReply(Guid id);
        ICollection<Reply> GetReplies(Guid id);
        ICollection<Reply> GetReplies();
        int GetReplyCount(Guid id);
        void Delete(Guid commentId);
        // void Update(Reply reply);
    }
}