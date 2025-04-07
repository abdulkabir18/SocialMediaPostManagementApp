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
            Console.WriteLine("1.\tCreate Post\n2.\tView Posts\n3.\tView Profile\n4.\tEdit Profile\n5.\tDelete Account\n6.\tLogout");
            string? opt = Console.ReadLine();
            switch (opt)
            {
                case "1":
                    AddPost(user.Id, user.FirstName + " " + user.LastName);
                    break;
                case "2":
                    ViewPosts(user);
                    break;
                case "3":
                    ViewProfile(user);
                    break;
                case "4":
                    EditProfile(user);
                    break;
                case "5":

                    break;
                case "6":
                    StartApp();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    UserStart();
                    break;
            }
        }

        void AddPost(Guid userId, string fullName)
        {
            Console.WriteLine("Create Your Post");
            Console.Write("Title: ");
            string title = Console.ReadLine()!;
            Console.Write("Content: ");
            string content = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(content) || string.IsNullOrEmpty(title))
            {
                Console.WriteLine("Content or title cannot be empty.");
                UserStart();
            }

            var post = new CreatePostRequestModel
            {
                Content = content,
                CreatedBy = fullName,
                SocialMediaUserId = userId,
                Title = title
            };

            var result = _postService.MakePost(post);
            if (result.Status)
            {
                Console.WriteLine("Post created successfully. {0}", result.Message);
            }
            else
            {
                Console.WriteLine("Error creating post: {0}", result.Message);
            }

            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
            UserStart();
        }

        void ViewPosts(SocialMediaUserDto mediauser)
        {
            var posts = _postService.ViewOthersPosts(mediauser.Id);
            if (posts.Count == 0)
            {
                Console.WriteLine("\nNo post available.");
            }
            else
            {
                foreach (var post in posts)
                {
                    Console.WriteLine($"{post.CreatedBy}\n{post.DateCreated.ToString()}\n{post.Title}\n{post.Content}\nLikes: {post.LikeCount}\t\tComments: {post.CommentCount}");
                    Console.WriteLine("\n1.\tLike\n2.\tComment\n3.\tView Comments\n4.\tBack to menu\nPress any other key to continue");
                    Console.Write("Select an option: ");
                    var opt = Console.ReadLine();
                    switch (opt)
                    {
                        case "1":
                            LikePost(post.Id, mediauser.FirstName + " " + mediauser.LastName);
                            break;
                        case "2":
                            Commet(post.Id, mediauser.FirstName + " " + mediauser.LastName);
                            break;
                        case "3":
                            ViewComment(post.Id);
                            break;
                        case "4":
                            UserStart();
                            break;
                        default:
                            continue;
                    }
                }
            }

            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
            UserStart();
        }

        void LikePost(Guid postId, string fullName)
        {
            var likePost = new MakeLikeRequestModel
            {
                CreatedBy = fullName,
                PostId = postId
            };
            var result = _likeService.LikePost(likePost);
            if (result.Status)
            {
                Console.WriteLine("Post liked successfully. {0}", result.Message);
            }
            else
            {
                Console.WriteLine("Error liking post: {0}", result.Message);
            }

            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
        }

        void Commet(Guid postId, string fullName)
        {
            Console.Write("Enter your comment: ");
            string comment = Console.ReadLine()!;
            if (string.IsNullOrWhiteSpace(comment))
            {
                Console.WriteLine("Comment cannot be empty.");
            }
            else
            {
                var commentPost = new CommentRequestModel
                {
                    CreatedBy = fullName,
                    PostId = postId,
                    Message = comment
                };
                var result = _commentService.AddComment(commentPost);
                if (result.Status)
                {
                    Console.WriteLine("Comment added successfully. {0}", result.Message);
                }
                else
                {
                    Console.WriteLine("Error adding comment: {0}", result.Message);
                }
            }

            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
            UserStart();
        }

        void ViewComment(Guid postId)
        {
            var comments = _commentService.GetComments(postId);
            if (comments.Count == 0)
            {
                Console.WriteLine("\nNo comment available.");
            }
            else
            {
                foreach (var comment in comments)
                {
                    Console.WriteLine($"{comment.CreatedBy}\n{comment.DateCreated.ToString()}\n{comment.Message}");
                }
            }

            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
            UserStart();
        }

        void ViewProfile(SocialMediaUserDto mediauser)
        {
            // var user = _socialMediaUserService.ViewSocialMediaUser(mediauser.Id);

            if (mediauser == null)
            {
                StartApp();
                return;
            }
            Console.WriteLine($"{mediauser.FirstName} {mediauser.LastName} ({mediauser.UserName})");

            var posts = _postService.ViewAllPosts(mediauser.Id);
            if (posts.Count == 0)
            {
                Console.WriteLine("\nNo post");
            }
            else
            {
                Console.WriteLine("\nViewing Posts");
                foreach (var post in posts)
                {
                    Console.WriteLine($"{post.CreatedBy}\n{post.DateCreated.ToString()}\n{post.Title}\n{post.Content}\nLikes: {post.LikeCount}\t\tComments: {post.CommentCount}");
                }
            }

            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
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

            // Console.Write("New Email (leave blank to keep current): ");
            // var email = Console.ReadLine();
            // if (!string.IsNullOrWhiteSpace(email))
            // {
            //     mediauser.Email = email;
            // }
            // else if (email != null && !email.EndsWith("@gmail.com") || email != null && email.Length < 20)
            // {
            //     Console.WriteLine("Invalid email");
            //     UserStart();
            // }



            var socialMediaUser = new EditSocialMediaUserRequestModel
            {
                Id = mediauser.Id,
                FirstName = mediauser.FirstName,
                LastName = mediauser.LastName,
                UserName = mediauser.UserName,
                Address = mediauser.Address
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

            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
            UserStart();
        }
    }
}