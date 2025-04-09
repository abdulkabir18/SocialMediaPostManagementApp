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
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IReplyRepository _replyRepository;

        public SocialMediaUserService()
        {
            _userRepository = new UserRepository();
            _socialMediaUserRepository = new SocialMediaUserRepository();
            _postRepository = new PostRepository();
            _commentRepository = new CommentRepository();
            _replyRepository = new ReplyRepository();
            _likeRepository = new LikeRepository();
        }

        public Result<string> DeleteMediaUser(DeleteSocialMediaUserRequestModel delete)
        {
            var mediaUser = _socialMediaUserRepository.GetMediaUser(delete.Email.ToLower());
            if (mediaUser != null)
            {
                bool checkPosts = _postRepository.GetPosts().Any(post => post.SocialMediaUserId == mediaUser.Id);
                if (!checkPosts)
                {
                    _userRepository.SoftDelete(mediaUser.Email);
                    _socialMediaUserRepository.SoftDelete(mediaUser.Email);

                    return new Result<string>
                    {
                        Status = true,
                        Data = mediaUser.Id.ToString(),
                        Message = "Account deleted successfully"
                    };
                }
                else
                {
                    var posts = _postRepository.GetPosts(mediaUser.Id);
                    foreach (var post in posts)
                    {
                        bool checkComment = _commentRepository.GetComments().Any(comment => comment.PostId == post.Id);
                        bool checkLike = _likeRepository.GetLikes().Any(like => like.PostId == post.Id);
                        if (checkLike)
                        {
                            _likeRepository.Delete(post.Id);
                        }

                        if (checkComment)
                        {
                            var comments = _commentRepository.GetComments(post.Id);
                            foreach (var comment in comments)
                            {
                                bool checkReply = _replyRepository.GetReplies().Any(reply => reply.CommentId == comment.Id);
                                if (checkLike)
                                {
                                    _replyRepository.Delete(comment.Id);
                                }
                                _commentRepository.Delete(comment.Id);
                            }
                        }

                        _postRepository.Delete(post.Id);
                    }

                    return new Result<string>
                    {
                        Status = true,
                        Data = mediaUser.Id.ToString(),
                        Message = "Account deleted successfully"
                    };
                }
            }

            return new Result<string>
            {
                Status = false,
                Data = null,
                Message = "Account not found"
            };
            // SocialMediaUser? user = _socialMediaUserRepository.GetMediaUser(id);
            // var _user = _userRepository.GetUsers().SingleOrDefault(a => a.Email == user?.Email);
            // if (user == null || _user == null)
            // {
            //     return new Result<string>
            //     {
            //         Status = false,
            //         Message = $"No details found for",
            //         Data = null
            //     };
            // }
            // _user.IsDelete = true;
            // _userRepository.Update(_user);

            // user.IsDelete = true;
            // _socialMediaUserRepository.Update(user);

            // return new Result<string>
            // {
            //     Status = true,
            //     Message = $"{user.FirstName} {user.LastName} account deleted successfully",
            //     Data = user.Id.ToString()
            // };
        }

        public Result<string> EditMediaUser(EditSocialMediaUserRequestModel edit)
        {
            var mediauser = _socialMediaUserRepository.GetMediaUser(edit.Id);
            if (mediauser == null)
            {
                return new Result<string>
                {
                    Status = false,
                    Message = $"No details found for {edit.FirstName} {edit.LastName}",
                    Data = null
                };
            }
            // var user = _userRepository.GetUsers().FirstOrDefault(a => a.Email == mediauser.Email);
            // if (user == null)
            // {
            //     return new Result<string>
            //     {
            //         Status = false,
            //         Message = $"No details found for {edit.FirstName} {edit.LastName}",
            //         Data = null
            //     };
            // }

            // bool isEmailExist = _userRepository.CheckIfEmailExist(user.Email.ToLower());
            // if (isEmailExist)
            // {
            //     return new Result<string>
            //     {
            //         Status = false,
            //         Message = $"Email is already associated with another account",
            //         Data = null
            //     };
            // }

            bool isUserNameExist = _socialMediaUserRepository.CheckIfUserNameExist(edit.UserName);
            if (mediauser.UserName == edit.UserName || isUserNameExist)
            {
                return new Result<string>
                {
                    Status = false,
                    Message = $"UserName is already taken",
                    Data = null
                };
            }


            mediauser.FirstName = edit.FirstName;
            mediauser.LastName = edit.LastName;
            mediauser.Address = edit.Address;
            mediauser.DateOfBirth = edit.DateOfBirth;
            mediauser.UserName = edit.UserName;

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
                    Message = "This password cant be use",
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
                Email = register.Email,
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

        public SocialMediaUserDto? ViewSocialMediaUser(string email)
        {
            var mediaUser = _socialMediaUserRepository.GetMediaUser(email);
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