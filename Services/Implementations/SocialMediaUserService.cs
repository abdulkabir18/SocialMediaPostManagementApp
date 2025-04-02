using SocialMediaPostManager.Context;
using SocialMediaPostManager.Dtos;
using SocialMediaPostManager.Models.Entities;
using SocialMediaPostManager.Models.Enum;
using SocialMediaPostManager.Repositories.Implenentations;
using SocialMediaPostManager.Repositories.Interfaces;
using SocialMediaPostManager.Services.Interfaces;

namespace SocialMediaPostManager.Services.Implementations
{
    public class SocialMediaUserService : ISocialMediaUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISocialMediaUserRepository _socialMediaUserRepository;

        public SocialMediaUserService()
        {
            _userRepository = new UserRepository();
            _socialMediaUserRepository = new SocialMediaUserRepository();
        }

        public Result<string> DeleteMediaUser(Guid id)
        {
            SocialMediaUser? user = _socialMediaUserRepository.GetMediaUser(id);
            var _user = _userRepository.GetUsers().SingleOrDefault(a => a.Email == user?.Email);
            if (user == null || _user == null)
            {
                return new Result<string>
                {
                    Status = false,
                    Message = $"No details found for",
                    Data = null
                };
            }
            _user.IsDelete = true;
            _userRepository.Update(_user);

            user.IsDelete = true;
            _socialMediaUserRepository.Update(user);

            return new Result<string>
            {
                Status = true,
                Message = $"{user.FirstName} {user.LastName} account deleted successfully",
                Data = user.Id.ToString()
            };
        }

        public Result<string> EditMediaUser(EditSocialMediaUserRequestModel edit)
        {
            var mediauser = _socialMediaUserRepository.GetMediaUser(edit.Id);
            var user = _userRepository.GetUsers().SingleOrDefault(a => a.Email == edit.Email.ToLower());
            if (mediauser == null || user == null)
            {
                return new Result<string>
                {
                    Status = false,
                    Message = $"No details found for {edit.FirstName} {edit.LastName}",
                    Data = null
                };
            }

            bool isEmailExist = _userRepository.CheckIfEmailExist(user.Email.ToLower());
            if (isEmailExist)
            {
                return new Result<string>
                {
                    Status = false,
                    Message = $"Email is already associated with another account",
                    Data = null
                };
            }

            bool isUserNameExist = _socialMediaUserRepository.CheckIfUserNameExist(edit.UserName);
            if (isUserNameExist)
            {
                return new Result<string>
                {
                    Status = false,
                    Message = $"UserName is already taken",
                    Data = null
                };
            }

            user.Email = edit.Email.ToLower();
            _userRepository.Update(user);

            mediauser.FirstName = edit.FirstName;
            mediauser.LastName = edit.LastName;
            mediauser.Address = edit.Address;
            mediauser.DateOfBirth = edit.DateOfBirth;
            mediauser.UserName = edit.UserName;
            mediauser.Email = edit.Email.ToLower();

            _socialMediaUserRepository.Update(mediauser);
            return new Result<string>
            {
                Status = true,
                Message = $"{mediauser.FirstName} {mediauser.LastName} account updated successfully",
                Data = mediauser.Id.ToString()
            };
        }

        public SocialMediaUserDto? GetSocialMediaUserByEmail(string email)
        {
            SocialMediaUser? user = _socialMediaUserRepository.GetMediaUsers().SingleOrDefault(a => a.Email == email.ToLower());
            if (user == null)
            {
                return null;
            }
            return new SocialMediaUserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Id = user.Id,
                Address = user.Address,
                Gender = user.Gender,
                UserName = user.UserName
            };
        }

        public Result<string> RegisterMediaUser(RegisterSocialMediaUserRequestModel register)
        {
            bool isEmailExist = _userRepository.CheckIfEmailExist(register.Email.ToLower());
            if (isEmailExist)
            {
                return new Result<string>
                {
                    Status = false,
                    Message = $"Email is already associated with another account",
                    Data = null
                };
            }

            bool isUserNameExist = _socialMediaUserRepository.CheckIfUserNameExist(register.UserName);
            if (isUserNameExist)
            {
                return new Result<string>
                {
                    Status = false,
                    Message = $"UserName is already taken",
                    Data = null
                };
            }

            string hashPassword = SociaMediapostManagerContext.HashPassword(register.Password);
            bool isPasswordExist = _userRepository.CheckIfPasswordExist(hashPassword);
            if (isPasswordExist)
            {
                return new Result<string>
                {
                    Status = false,
                    Message = $"Password is already exists",
                    Data = null
                };
            }

            var user = new User
            {
                Email = register.Email,
                Password = hashPassword,
                Role = Role.User
            };
            var mediaUser = new SocialMediaUser
            {
                Email = register.Email.ToLower(),
                FirstName = register.FirstName,
                LastName = register.LastName,
                Address = register.Address,
                DateOfBirth = register.DateOfBirth,
                Gender = register.Gender,
                UserName = register.UserName
            };
            _userRepository.Persist(user);
            _socialMediaUserRepository.Persist(mediaUser);
            return new Result<string>
            {
                Data = mediaUser.Id.ToString(),
                Message = $"{mediaUser.FirstName} {mediaUser.LastName} register succesfully",
                Status = true
            };

        }

        public ICollection<SocialMediaUserDto> ViewAllSocialMediaUsers()
        {
            List<SocialMediaUserDto> mediaUsers = new List<SocialMediaUserDto>();
            foreach (SocialMediaUser mediaUser in _socialMediaUserRepository.GetMediaUsers())
            {
                var _mediaUser = new SocialMediaUserDto
                {
                    Email = mediaUser.Email,
                    FirstName = mediaUser.FirstName,
                    LastName = mediaUser.LastName,
                    UserName = mediaUser.UserName,
                    Address = mediaUser.Address,
                    DateOfBirth = mediaUser.DateOfBirth,
                    Gender = mediaUser.Gender,
                    Id = mediaUser.Id
                };
                mediaUsers.Add(_mediaUser);
            }
            return mediaUsers;
        }

        public SocialMediaUserDto? ViewSocialMediaUser(string userName)
        {
            var mediaUser = _socialMediaUserRepository.GetMediaUser(userName);
            if (mediaUser != null)
            {
                return new SocialMediaUserDto
                {
                    Email = mediaUser.Email,
                    FirstName = mediaUser.FirstName,
                    LastName = mediaUser.LastName,
                    UserName = mediaUser.UserName,
                    Address = mediaUser.Address,
                    DateOfBirth = mediaUser.DateOfBirth,
                    Gender = mediaUser.Gender,
                    Id = mediaUser.Id
                };
            }
            return null;
        }

        public SocialMediaUserDto? ViewSocialMediaUser(Guid id)
        {
            var mediaUser = _socialMediaUserRepository.GetMediaUser(id);
            if (mediaUser != null)
            {
                return new SocialMediaUserDto
                {
                    Email = mediaUser.Email,
                    FirstName = mediaUser.FirstName,
                    LastName = mediaUser.LastName,
                    UserName = mediaUser.UserName,
                    Address = mediaUser.Address,
                    DateOfBirth = mediaUser.DateOfBirth,
                    Gender = mediaUser.Gender,
                    Id = mediaUser.Id
                };
            }
            return null;
        }
    }
}