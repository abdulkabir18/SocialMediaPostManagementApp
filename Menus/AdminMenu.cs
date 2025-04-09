using SocialMediaPostManager.Dtos;

namespace SocialMediaPostManager.Menus
{
    public partial class AppMenu
    {
        // view all mediaUsers
        // view mediaUser by email
        // view all posts
        // view all posts by mediaUserId(email)
        // view post by id
        // view all comments
        // view all comments by postId
        // view comment by id
        // view all likes
        // view all likes by postId
        // view like by id
        // view all replies
        // view all replies by commentId
        // view reply by id

        public void AdminStart()
        {
            const string adminUsername = "Admin";
            Console.WriteLine("Welcome {0}", adminUsername);
        // Console.WriteLine("You are logged in as an admin. You can view all media users and their posts.");
        label:
            Console.WriteLine("\n1.\tView all media users profile\n2.\tView media user profile by email\n3.\tDeactivate media user\n4.\tView all posts\n5.\tExist");
            Console.Write("\nEnter your option: ");
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    ViewAllMediaUsers();
                    break;
                case "2":
                    ViewMediaUserByEmail();
                    break;
                case "3":
                    DeactivateMediaUser();
                    break;
                case "4":
                    ViewAllMediaPosts();
                    break;
                case "5":
                    Console.WriteLine("Exiting...");
                    StartApp();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    goto label;
            }
        }
        public void ViewAllMediaUsers()
        {
            var mediaUsers = _socialMediaUserService.ViewAllSocialMediaUsers();
            if (mediaUsers.Count == 0)
            {
                Console.WriteLine("No media users found.");
            }
            else
            {
                Console.WriteLine("View media users: ");
                foreach (var mediaUser in mediaUsers)
                {
                    Console.WriteLine($"FirstName: {mediaUser.FirstName}\nLastName: {mediaUser.LastName}\nEmail: {mediaUser.Email}\nAddress: {mediaUser.Address}");

                    ViewMediaPosts(mediaUser.Id);
                }
            }

            Console.WriteLine("\nPress Enter to return to menu.");
            Console.ReadLine();
            AdminStart();
        }

        public void ViewMediaUser(string email)
        {
            var mediaUser = _socialMediaUserService.ViewSocialMediaUser(email);

            if (mediaUser == null)
            {
                Console.WriteLine($"Media user not found with Email: {email}");
            }
            else
            {
                Console.WriteLine($"FirstName: {mediaUser.FirstName}\nLastName: {mediaUser.LastName}\nEmail: {mediaUser.Email}\nAddress: {mediaUser.Address}");

                ViewMediaPosts(mediaUser.Id);
            }
        }

        public void DeactivateMediaUser()
        {
            string confirmEmail;
            do
            {
                Console.Write("Enter email of the user you want to deactivate: ");
                confirmEmail = Console.ReadLine()!;

                if (string.IsNullOrEmpty(confirmEmail) || string.IsNullOrWhiteSpace(confirmEmail))
                {
                    Console.WriteLine("Invalid email");
                }
            }
            while (string.IsNullOrEmpty(confirmEmail) || string.IsNullOrWhiteSpace(confirmEmail));

            var getMediaUser = _socialMediaUserService.GetSocialMediaUserByEmail(confirmEmail);

            if (getMediaUser != null)
            {
                Console.WriteLine("Account deleting.......");
                var delete = new DeleteSocialMediaUserRequestModel
                {
                    Email = getMediaUser.Email
                };
                var result = _socialMediaUserService.DeleteMediaUser(delete);
                if (result.Status)
                {
                    Console.WriteLine($"{result.Message}");
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
            AdminStart();
        }

        public void ViewAllMediaPosts()
        {
            var posts = _postService.ViewAllPosts();

            if (posts.Count == 0)
            {
                Console.WriteLine("No post found for this user");
            }
            else
            {
                Console.WriteLine("Viewing media users post: ");
                foreach (var post in posts)
                {
                    Console.WriteLine($"\n{post.CreatedBy}\n{post.DateCreated.ToString()}\n{post.Title}\n{post.Content}\nLikes: {post.LikeCount}\t\tComments: {post.CommentCount}");

                    Console.WriteLine("\nPress enter to continue viewing post");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("\nPress Enter to return to menu.");
            Console.ReadLine();
            AdminStart();
        }
        public void ViewMediaUserByEmail()
        {
            string email;
            do
            {
                Console.Write("Enter user email: ");
                email = Console.ReadLine()!;

                if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("Email is required");
                }
            }
            while (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email));

            ViewMediaUser(email);

            Console.WriteLine("\nPress Enter to return to menu.");
            Console.ReadLine();
            AdminStart();
        }

        public void ViewMediaPosts(Guid mediaUserId)
        {
            var posts = _postService.ViewAllPosts(mediaUserId);
            if (posts.Count == 0)
            {
                Console.WriteLine("No post found for this user");
            }
            else
            {
                Console.WriteLine("View media user posts: ");
                foreach (var post in posts)
                {
                    Console.WriteLine($"{post.CreatedBy}\n{post.DateCreated.ToString()}\n{post.Title}\n{post.Content}\nLikes: {post.LikeCount}\t\tComments: {post.CommentCount}");

                    Console.Write("\n1.\tView post comments\n2.\tView post likes\n3.\tGo back\n4.\tGo to menu\nPress any other key to continue");
                    var opt = Console.ReadLine();

                    if (opt == "1")
                    {
                        ViewMediaPostComments(post.Id);
                    }
                    else if (opt == "2")
                    {
                        ViewLikes(post.Id);
                    }
                    else if (opt == "3")
                    {
                        return;
                    }
                    else if (opt == "4")
                    {
                        AdminStart();
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            Console.WriteLine("\nPress Enter to go back.");
            Console.ReadLine();
        }

        public void ViewMediaPostComments(Guid postId)
        {
            var comments = _commentService.GetComments(postId);
            if (comments.Count == 0)
            {
                Console.WriteLine("No comment found.");
            }
            else
            {
                Console.WriteLine("View post comment: ");
                foreach (var comment in comments)
                {
                    Console.WriteLine($"\n{comment.CreatedBy}\n{comment.DateCreated.ToString()}\n{comment.Message}\nReplies: {comment.ReplyCount}\n");

                    Console.Write("\n1.\tView comment replies\n2.\tGo back\n3.\tGo to menu\nPress any other key to continue");
                    var opt = Console.ReadLine();
                    if (opt == "1")
                    {
                        ViewMediaCommentReplies(comment.Id);
                    }
                    else if (opt == "2")
                    {
                        return;
                    }
                    else if (opt == "3")
                    {
                        AdminStart();
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            Console.WriteLine("\nPress Enter to go back.");
            Console.ReadLine();
        }

        public void ViewMediaCommentReplies(Guid commentId)
        {
            var replies = _replyService.GetReplies(commentId);
            if (replies.Count == 0)
            {
                Console.WriteLine("No reply found");
            }
            else
            {
                Console.WriteLine("Viewing comment replies: ");
                foreach (var reply in replies)
                {
                    Console.WriteLine($"\n{reply.CreatedBy}\n{reply.DateCreated.ToString()}\n{reply.Message}\n");
                }
            }

            Console.WriteLine("\nPress Enter to go back.");
            Console.ReadLine();
        }

    }
}