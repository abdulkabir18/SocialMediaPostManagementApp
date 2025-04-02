using SocialMediaPostManager.Dtos;

namespace SocialMediaPostManager.Services.Interfaces
{
    public interface IUserService
    {
        Result<UserDto>? Login(LoginUserRequsetModel loginUser);
        // UserDto? GetUser(string email, string password);
        Result<UserDto> GetCurrentUser();
        ICollection<UserDto> GetUsers();
    }
}