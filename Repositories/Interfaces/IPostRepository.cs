using SocialMediaPostManager.Models.Entities;

namespace SocialMediaPostManager.Repositories.Interfaces
{
    public interface IPostRepository
    {
        void Persist(Post post);
        void Update(Post post);
        ICollection<Post> GetPosts();
        ICollection<Post> GetPosts(Guid id);
        ICollection<Post> GetOthersPosts(Guid id);
        Post? GetPost(Guid id);
    }
}