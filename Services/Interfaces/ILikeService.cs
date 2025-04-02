using SocialMediaPostManager.Dtos;

namespace SocialMediaPostManager.Services.Interfaces
{
    public interface ILikeService
    {
        Result<string> LikePost(MakeLikeRequestModel makeLike);
        ICollection<LikeDto> ViewAllLikes(Guid id);
        ICollection<LikeDto> ViewAllLikes();
    }
}