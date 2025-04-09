using SocialMediaPostManager.Dtos;

namespace SocialMediaPostManager.Services.Interfaces
{
    public interface ISocialMediaUserService
    {
        Result<string> RegisterMediaUser(RegisterSocialMediaUserRequestModel register);
        Result<string> EditMediaUser(EditSocialMediaUserRequestModel edit);
        Result<string> DeleteMediaUser(DeleteSocialMediaUserRequestModel delete);
        SocialMediaUserDto? ViewSocialMediaUser(string userName);
        SocialMediaUserDto? ViewSocialMediaUser(Guid id);
        SocialMediaUserDto? GetSocialMediaUserByEmail(string email);
        ICollection<SocialMediaUserDto> ViewAllSocialMediaUsers();
    }
}