using SocialMediaPostManager.Models.Enum;

namespace SocialMediaPostManager.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public Role Role { get; set; }
    }
    public class LoginUserRequsetModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}