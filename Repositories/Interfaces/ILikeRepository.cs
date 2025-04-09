using SocialMediaPostManager.Models.Entities;

namespace SocialMediaPostManager.Repositories.Interfaces
{
    public interface ILikeRepository
    {
        void Persist(Like like);
        void Delete(Guid id);
        ICollection<Like> GetLikes(Guid id);
        ICollection<Like> GetLikes();
        int GetLikeCount(Guid postId);
    }
}