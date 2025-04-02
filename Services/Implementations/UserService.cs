using SocialMediaPostManager.Context;
using SocialMediaPostManager.Dtos;
using SocialMediaPostManager.Repositories.Implenentations;
using SocialMediaPostManager.Repositories.Interfaces;
using SocialMediaPostManager.Services.Interfaces;

namespace SocialMediaPostManager.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public static UserDto? CurrentUser { get; set; } = null;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        // public UserDto? GetUser(string email, string password)
        // {
        //     throw new NotImplementedException();
        // }

        public ICollection<UserDto> GetUsers()
        {
            List<UserDto> users = [];
            foreach (var user in _userRepository.GetUsers())
            {
                users.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Password = user.Password,
                    Role = user.Role
                });
            }
            return users;
        }

        public Result<UserDto>? Login(LoginUserRequsetModel loginUser)
        {
            string password = SociaMediapostManagerContext.HashPassword(loginUser.Password);
            var user = _userRepository.GetUser(loginUser.Email.ToLower(), password);
            if (user == null)
            {
                return new Result<UserDto>
                {
                    Status = false,
                    Message = "Invalid email or password",
                    Data = null
                };
            }
            var userDto = new UserDto
            {
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Id = user.Id
            };
            CurrentUser = userDto;
            return new Result<UserDto>
            {
                Status = true,
                Message = "Login successfully",
                Data = userDto
            };
        }

        public Result<UserDto> GetCurrentUser()
        {
            if (CurrentUser == null)
            {
                return new Result<UserDto>
                {
                    Data = null,
                    Message = "Please login",
                    Status = false
                };
            }
            return new Result<UserDto>
            {
                Data = CurrentUser,
                Message = $"You are welcome",
                Status = true
            };
        }

    }


}