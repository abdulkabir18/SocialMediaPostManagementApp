using System.Text.RegularExpressions;
using SocialMediaPostManager.Dtos;
using SocialMediaPostManager.Models.Entities;
using SocialMediaPostManager.Models.Enum;
using SocialMediaPostManager.Services.Implementations;
using SocialMediaPostManager.Services.Interfaces;

namespace SocialMediaPostManager.Menus
{
    public partial class AppMenu
    {
        private readonly IUserService _userService;
        private readonly ISocialMediaUserService _socialMediaUserService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;

        public AppMenu()
        {
            _userService = new UserService();
            _socialMediaUserService = new SocialMediaUserService();
            _postService = new PostService();
            _commentService = new CommentService();
            _likeService = new LikeService();
        }

        public void StartApp()
        {
            Console.WriteLine("1.\tRegister Account\n2.\tLogin");
            string? opt = Console.ReadLine();
            if (string.IsNullOrEmpty(opt))
            {
                Console.Beep();
                Console.WriteLine("Option cant be null or empty");
                StartApp();
            }
            else if (opt == "1")
            {
                RegisterMenu();
            }
            else if (opt == "2")
            {
                LoginMenu();
            }
            else
            {
                Console.Beep();
                Console.WriteLine("Invalid input");
                StartApp();
            }
        }

        public void RegisterMenu()
        {
            string firstName;
            do
            {
                Console.Write("FirstName:\t");
                firstName = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrEmpty(firstName) || string.IsNullOrWhiteSpace(firstName))
                {
                    Console.Beep();
                    Console.WriteLine("FirstName is required. Please enter a valid input");
                }
            }
            while (string.IsNullOrEmpty(firstName) || string.IsNullOrWhiteSpace(firstName));

            string lastName;
            do
            {
                Console.Write("LastName:\t");
                lastName = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrEmpty(lastName) || string.IsNullOrWhiteSpace(lastName))
                {
                    Console.Beep();
                    Console.WriteLine("LastName is required. Please enter a valid input");
                }
            }
            while (string.IsNullOrEmpty(lastName) || string.IsNullOrWhiteSpace(lastName));

            string address;
            do
            {
                Console.Write("Address:\t");
                address = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(address) || string.IsNullOrWhiteSpace(address))
                {
                    Console.Beep();
                    Console.WriteLine("Address is required");
                }
            }
            while (string.IsNullOrEmpty(address) || string.IsNullOrWhiteSpace(address));

            string email;
            string pattern = @"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com)$";
            do
            {
                Console.Write("Email:\t");
                email = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, pattern))
                {
                    Console.Beep();
                    Console.WriteLine("Email is required and Please enter a valid email");
                }
            }
            while (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, pattern));

            // Console.Write("Email:\t");
            // string email = Console.ReadLine() ?? string.Empty;
            // if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
            // {
            //     Console.Beep();
            //     Console.WriteLine("Email is required");
            //     RegisterMenu();
            // }
            // else if (!email.EndsWith("@gmail.com") || email.Length < 20)
            // {
            //     Console.WriteLine("Invalid email");
            //     RegisterMenu();
            // }

            string userName;
            do
            {
                Console.Write("UserName:\t");
                userName = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName) || !userName.All(char.IsLetterOrDigit))
                {
                    Console.Beep();
                    Console.WriteLine("Username is required");
                }
            }
            while (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName) || !userName.All(char.IsLetterOrDigit));

            // Console.Write("UserName:\t");
            // string userName = Console.ReadLine() ?? string.Empty;
            // if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName))
            // {
            //     Console.Beep();
            //     Console.WriteLine("Username is required");
            //     RegisterMenu();
            // }
            // else if (!userName.All(char.IsLetterOrDigit))
            // {
            //     Console.WriteLine("Username must only contain letters and numbers");
            //     RegisterMenu();
            // }

            string password;
            do
            {
                Console.Write("Password:\t");
                password = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password)
                || password.Length < 8 || !password.Any(char.IsLower) || !password.Any(char.IsUpper) || !password.Any(char.IsDigit))
                {
                    Console.Beep();
                    Console.WriteLine("Password is required. Please enter a valid password");
                }
            }
            while (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password)
            || password.Length < 8 || !password.Any(char.IsLower) || !password.Any(char.IsUpper) || !password.Any(char.IsDigit));

            // Console.Write("Password:\t");
            // string password = Console.ReadLine() ?? string.Empty;
            // if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            // {
            //     Console.Beep();
            //     Console.WriteLine("Password is required");
            //     RegisterMenu();
            // }
            // else if (password.Length < 8 || !password.Any(char.IsLower) || !password.Any(char.IsUpper) || !password.Any(char.IsDigit))
            // {
            //     Console.WriteLine("Invalid password");
            //     RegisterMenu();
            // }

            Console.Write("Date of birth (yyyy-mm-dd):\t");
            string? dOB = Console.ReadLine();
            DateOnly dateOfBirth;
            while (!DateOnly.TryParse(dOB, out dateOfBirth))
            {
                Console.WriteLine("D.O.B not in correct format");
                Console.Write("Date of birth (yyyy-mm-dd):\t");
            }

            foreach (Gender gender in Enum.GetValues(typeof(Gender)))
            {
                Console.WriteLine($"Press {(int)gender}. {gender}");
            }
            Console.Write("Gender:\t");
            string? input = Console.ReadLine();
            int choice;
            while (!int.TryParse(input, out choice) || choice > 2)
            {
                Console.WriteLine("Input from (1-2)");
                Console.Write("Gender:\t");
            }

            var getgender = "";
            if (choice == 1)
            {
                getgender = Gender.Male.ToString();
            }
            else
            {
                getgender = Gender.Female.ToString();
            }

            RegisterSocialMediaUserRequestModel registerSocial = new RegisterSocialMediaUserRequestModel
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                UserName = userName,
                Gender = (Gender)Enum.Parse(typeof(Gender), getgender),
                DateOfBirth = dateOfBirth
            };

            var response = _socialMediaUserService.RegisterMediaUser(registerSocial);
            if (!response.Status)
            {
                Console.WriteLine(response.Message + "\nTry Again!!!");
                StartApp();
            }
            else
            {
                Console.WriteLine(response.Message);
                StartApp();
            }
        }

        public void LoginMenu()
        {
            Console.Write("Email:\t");
            string email = Console.ReadLine()!;
            Console.Write("Password:\t");
            string password = Console.ReadLine()!;
            if (email == null || password == null)
            {
                Console.WriteLine("email or password cant be null");
                LoginMenu();
                return;
            }
            Result<UserDto>? response = _userService.Login(new LoginUserRequsetModel { Email = email, Password = password });
            if (response == null || response.Data == null)
            {
                Console.WriteLine($"{response?.Message}\nTry Again!!!");
                StartApp();
            }
            else
            {
                if (response.Data.Role == Role.Admin)
                {
                    Console.WriteLine(response.Message);
                    AdminStart();
                }
                else if (response.Data.Role == Role.User)
                {
                    Console.WriteLine(response.Message);
                    UserStart();
                }
            }
        }
    }
}