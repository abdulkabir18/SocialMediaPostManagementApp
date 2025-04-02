using SocialMediaPostManager.Models.Entities;

namespace SocialMediaPostManager.Repositories.Interfaces
{
    public interface ILikeRepository
    {
        void Persist(Like like);
        ICollection<Like> GetLikes(Guid id);
        ICollection<Like> GetLikes();
        int GetLikeCount(Guid id);
    }
}