using SocialMediaPostManager.Dtos;

namespace SocialMediaPostManager.Services.Interfaces
{
    public interface IPostService
    {
        Result<string> MakePost(CreatePostRequestModel createPost);
        Result<string> EditPost(EditPostRequestModel editPost);
        PostDto? ViewPost(Guid id);
        ICollection<PostDto> ViewAllPosts();
        ICollection<PostDto> ViewAllPosts(Guid id);
        ICollection<PostDto> ViewOthersPosts(Guid id);
    }
}