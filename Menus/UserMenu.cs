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
                    DeleteProfile(user.Email);
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

            string title;
            string content;

            do
            {
                Console.Write("Title: ");
                title = Console.ReadLine()!;
                Console.Write("Content: ");
                content = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(content) || string.IsNullOrEmpty(title))
                {
                    Console.WriteLine("Content and title cannot be empty.");
                }
            }
            while (string.IsNullOrWhiteSpace(content) || string.IsNullOrEmpty(title));

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
                Console.WriteLine(result.Message);
            }
            else
            {
                Console.WriteLine("Error creating post: {0}", result.Message);
            }

            Console.WriteLine("Press Enter to go back.");
            Console.ReadLine();
            UserStart();
        }

        void ViewPosts(SocialMediaUserDto mediauser)
        {
            int postIndex = 0;
        label:
            var posts = _postService.ViewOthersPosts(mediauser.Id).ToList();
            if (posts.Count == 0)
            {
                Console.WriteLine("\nNo post available.");
            }
            else
            {
                for (int i = postIndex; i < posts.Count; i++)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{posts[i].CreatedBy}\n{posts[i].DateCreated.ToString()}\n{posts[i].Title}\n{posts[i].Content}\nLikes: {posts[i].LikeCount}\t\tComments: {posts[i].CommentCount}");
                    Console.WriteLine("\n1.\tLike\n2.\tComment\n3.\tView Comments\n4.\tView Likes\n5.\tBack to menu\nPress any other key to continue");
                    Console.Write("Enter your option: ");
                    var opt = Console.ReadLine();
                    if (opt == "1")
                    {
                        LikePost(posts[i].Id, mediauser.FirstName + " " + mediauser.LastName);
                        goto label;
                    }
                    else if (opt == "2")
                    {
                        CommentPost(posts[i].Id, mediauser.FirstName + " " + mediauser.LastName);
                        goto label;
                    }
                    else if (opt == "3")
                    {
                        ViewComments(posts[i].Id, mediauser);
                        goto label;
                    }
                    else if (opt == "4")
                    {
                        ViewLikes(posts[i].Id);
                        goto label;
                    }
                    else if (opt == "5")
                    {
                        UserStart();
                        break;
                    }
                    else
                    {
                        postIndex += 1;
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

            Console.WriteLine("Press Enter to go back.");
            Console.ReadLine();
        }

        void CommentPost(Guid postId, string fullName)
        {
            string comment;
            do
            {
                Console.Write("Enter your comment: ");
                comment = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(comment))
                {
                    Console.WriteLine("Comment is requried.");
                }
            }
            while (string.IsNullOrWhiteSpace(comment));

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

            Console.WriteLine("\nPress Enter to go back.");
            Console.ReadLine();
        }

        void ViewComments(Guid postId, SocialMediaUserDto mediauser)
        {
            int commentIndex = 0;
        label:
            var comments = _commentService.GetComments(postId).ToList();
            if (comments.Count == 0)
            {
                Console.WriteLine("\nNo comment available.");
            }
            else
            {
                Console.WriteLine("\nViewing Comments");
                for (int i = commentIndex; i < comments.Count; i++)
                {
                    Console.WriteLine($"\n{comments[i].CreatedBy}\n{comments[i].DateCreated.ToString()}\n{comments[i].Message}\nReplies: {comments[i].ReplyCount}\n");

                    Console.WriteLine("\n1.\tReply\n2.\tView Replies\n3.\tBack to menu\nPress any other key to continue");
                    Console.Write("Enter your option: ");
                    var opt = Console.ReadLine();
                    if (opt == "1")
                    {
                        ReplyComment(comments[i].Id, mediauser);
                        goto label;
                    }
                    else if (opt == "2")
                    {
                        ViewReplies(comments[i].Id);
                        goto label;
                    }
                    else if (opt == "3")
                    {
                        UserStart();
                        break;
                    }
                    else
                    {
                        commentIndex += 1;
                        continue;
                    }
                }
            }

            Console.WriteLine("Press Enter to return to menu.");
            Console.ReadLine();
        }

        void ReplyComment(Guid commentId, SocialMediaUserDto mediauser)
        {
            string message;
            do
            {
                Console.Write("Enter your reply: ");
                message = Console.ReadLine()!;
                if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
                {
                    Console.WriteLine("Reply is required");
                }
            }
            while (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message));

            var reply = new CreateReplyRequestModel
            {
                Message = message,
                CommentId = commentId,
                CreatedBy = mediauser.FirstName + " " + mediauser.LastName,
                MediaUserId = mediauser.Id
            };
            var result = _replyService.AddReply(reply);
            if (result.Status)
            {
                Console.WriteLine(result.Message);
            }
            else
            {
                Console.WriteLine("Error adding reply: {0}", result.Message);
            }

            Console.WriteLine("Press Enter to go back.");
            Console.ReadLine();
        }

        void ViewReplies(Guid commentId)
        {
            var replies = _replyService.GetReplies(commentId);
            if (replies.Count == 0)
            {
                Console.WriteLine("\nNo reply available.");
            }
            else
            {
                Console.WriteLine("\nViewing Replies");
                foreach (var reply in replies)
                {
                    Console.WriteLine($"\n{reply.CreatedBy}\n{reply.DateCreated.ToString()}\n{reply.Message}\n");
                }
            }

            Console.WriteLine("Press Enter to go back.");
            Console.ReadLine();
        }

        void ViewLikes(Guid postId)
        {
            var likes = _likeService.ViewAllLikes(postId);
            if (likes.Count == 0)
            {
                Console.WriteLine("\nNo like found.");
            }
            else
            {
                Console.WriteLine("\nViewing Likes");
                foreach (var like in likes)
                {
                    Console.WriteLine($"\n{like.CreatedBy}\n");
                }
            }

            Console.WriteLine("Press Enter to go back.");
            Console.ReadLine();
        }

        void ViewProfile(SocialMediaUserDto mediauser)
        {
            if (mediauser == null)
            {
                StartApp();
                return;
            }
            Console.WriteLine($"{mediauser.FirstName} {mediauser.LastName} ({mediauser.UserName})");

            int postIndex = 0;
        label:
            var posts = _postService.ViewAllPosts(mediauser.Id).ToList();
            if (posts.Count == 0)
            {
                bool input;
                int opt;
                Console.WriteLine("\nNo post");
                do
                {
                    Console.Write("Did you want to add post (1 for Yes and 2 for No)\nEnter your option: ");
                    input = int.TryParse(Console.ReadLine(), out opt);
                }
                while (!input);
                {
                    Console.WriteLine("Invalid input");
                }

                if (opt == 1)
                {
                    AddPost(mediauser.Id, mediauser.FirstName + " " + mediauser.LastName);
                }
            }
            else
            {
                Console.WriteLine("\nViewing Posts");
                for (int i = postIndex; i < posts.Count; i++)
                {
                    Console.WriteLine();
                    Console.WriteLine($"\n{posts[i].DateCreated.ToString()}\n{posts[i].Title}\n{posts[i].Content}\nLikes: {posts[i].LikeCount}\t\tComments: {posts[i].CommentCount}");
                    Console.WriteLine("\n1.\tView comments\n2.\tView likes\n3.\tGo back to menu\nPress any other key to continue");
                    Console.Write("Enter your option: ");
                    var opt = Console.ReadLine();
                    if (opt == "1")
                    {
                        ViewComments(posts[i].Id, mediauser);
                        goto label;
                    }
                    else if (opt == "2")
                    {
                        ViewLikes(posts[i].Id);
                        goto label;
                    }
                    else if (opt == "3")
                    {
                        UserStart();
                        break;
                    }
                    else
                    {
                        postIndex += 1;
                        continue;
                    }
                }
            }

            Console.WriteLine("\nPress Enter to return to menu.");
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

            var socialMediaUser = new EditSocialMediaUserRequestModel
            {
                Id = mediauser.Id,
                FirstName = mediauser.FirstName,
                LastName = mediauser.LastName,
                UserName = mediauser.UserName,
                Address = mediauser.Address,
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

        void DeleteProfile(string email)
        {
            Console.WriteLine("Are you sure you want to delete your account");
        label:
            Console.Write("\ny for Yes\nn for No\nEnter your choice: ");
            bool input = char.TryParse(Console.ReadLine(), out char opt);
            if (!input)
            {
                Console.WriteLine("Invalid input\nyou want to delete your account");
                goto label;
            }

            if (opt == 'y')
            {
                string confirmEmail;
                do
                {
                    Console.Write("Enter your email: ");
                    confirmEmail = Console.ReadLine()!;

                    if (string.IsNullOrEmpty(confirmEmail) || string.IsNullOrWhiteSpace(confirmEmail))
                    {
                        Console.WriteLine("Invalid email");
                    }
                }
                while (string.IsNullOrEmpty(confirmEmail) || string.IsNullOrWhiteSpace(confirmEmail));

                if (email == confirmEmail)
                {
                    Console.WriteLine("Account deleting.......");
                    var delete = new DeleteSocialMediaUserRequestModel
                    {
                        Email = email
                    };
                    var result = _socialMediaUserService.DeleteMediaUser(delete);
                    if (result.Status)
                    {
                        Console.WriteLine($"Media user account deleted. {result.Message}");
                    }
                    else
                    {
                        Console.WriteLine($"Account delete error: {result.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect email");
                }
            }

            Console.WriteLine("\nPress Enter to return to menu.");
            Console.ReadLine();
            UserStart();
        }
    }
}