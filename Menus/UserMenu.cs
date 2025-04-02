using SocialMediaPostManager.Dtos;

namespace SocialMediaPostManager.Menus
{
    public partial class AppMenu
    {
        public void UserStart()
        {
            Result<UserDto> currentUser = _userService.GetCurrentUser();
            if (currentUser.Data == null)
            {
                Console.WriteLine("You are not logged in\n{0}", currentUser.Message);
                return;
            }
            SocialMediaUserDto? user = _socialMediaUserService.GetSocialMediaUserByEmail(currentUser.Data.Email);
            if (user == null)
            {
                Console.WriteLine("No match details");
                return;
            }
            Console.WriteLine("\n" + currentUser.Message + " " + user.UserName);
            Console.WriteLine("1.\tView Profile\n2.\tEdit Profile\n3.\tDelete Account\n4.\tLogout");
            string? opt = Console.ReadLine();
            switch (opt)
            {
                case "1":
                    ViewProfile(user);
                    break;
                case "2":
                    EditProfile(user);
                    break;
                case "3":

                    break;
                case "4":

                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        void ViewProfile(SocialMediaUserDto mediauser)
        {
            var user = _socialMediaUserService.ViewSocialMediaUser(mediauser.Id);

            if (user == null)
            {
                StartApp();
                return;
            }
            Console.WriteLine($"{user.FirstName} {user.LastName} ({user.UserName})");

            var posts = _postService.ViewAllPosts(mediauser.Id);
            if (posts.Count == 0)
            {
                Console.WriteLine("No post");
            }
            else
            {
                foreach (var post in posts)
                {
                    Console.WriteLine($"{post.CreatedBy}\n{post.DateCreated.ToString()}\n{post.Title}\n{post.Content}\n{post.LikeCount}\t\t{post.CommentCount}");
                }
            }
            UserStart();
        }

        void EditProfile(SocialMediaUserDto mediauser)
        {
            Console.WriteLine("Edit details of {0}", mediauser.UserName);

            Console.Write("New FirstName (leave blank to keep current): ");
            var firstName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                mediauser.FirstName = firstName;
            }

            Console.Write("New LastName (leave blank to keep current): ");
            var lastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                mediauser.LastName = lastName;
            }

            Console.Write("New UserName (leave blank to keep current): ");
            var userName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(userName))
            {
                mediauser.UserName = userName;
            }
            // else if (userName.Length < 3 || userName.Length > 15)
            // {
            //     Console.WriteLine("Username must be between 3 and 15 characters");
            //     UserStart();
            // }
            // else if (!userName.All(char.IsLetterOrDigit))
            // {
            //     Console.WriteLine("Username must only contain letters and numbers");
            //     UserStart();
            // }

            Console.Write("New UserName (leave blank to keep current): ");
            var email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
            {
                mediauser.Email = email;
            }
            else if (email != null && !email.EndsWith("@gmail.com") || email != null && email.Length < 20)
            {
                Console.WriteLine("Invalid email");
                UserStart();
            }



            var socialMediaUser = new EditSocialMediaUserRequestModel
            {
                Id = mediauser.Id,
                FirstName = mediauser.FirstName,
                LastName = mediauser.LastName,
                UserName = mediauser.UserName,
                Email = mediauser.Email,
            };
            var result = _socialMediaUserService.EditMediaUser(socialMediaUser);
            if (result.Status)
            {
                Console.WriteLine($"Profile updated successfully.\n{result.Message}");
            }
            else
            {
                Console.WriteLine("Error updating profile: {0}", result.Message);
            }

            UserStart();
        }
    }
}