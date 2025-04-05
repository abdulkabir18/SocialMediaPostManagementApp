using MySql.Data.MySqlClient;
using SocialMediaPostManager.Context;
using SocialMediaPostManager.Models.Entities;
using SocialMediaPostManager.Models.Enum;
using SocialMediaPostManager.Repositories.Interfaces;

namespace SocialMediaPostManager.Repositories.Implenentations
{
    public class UserRepository : IUserRepository
    {
        private readonly SociaMediapostManagerContext _sociaMediapostManagerContext;

        public UserRepository()
        {
            _sociaMediapostManagerContext = new SociaMediapostManagerContext();
        }

        public User? GetUser(string email, string password)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id,Email, Password, Role, IsDelete from users where (Email = @Email && Password = @Password)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Email", email);
            sqlCommand.Parameters.AddWithValue("@Password", password);
            var reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                    Email = reader["Email"].ToString() ?? "",
                    Password = reader["Password"].ToString() ?? "",
                    Role = (Role)Enum.Parse(typeof(Role), reader["Role"].ToString() ?? ""),
                    IsDelete = bool.Parse(reader["IsDelete"].ToString() ?? "")
                };
            }
            return null;
        }

        public bool CheckIfEmailExist(string email)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Email from users where (Email = @Email)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Email", email);
            var reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                return true;
            }
            return false;
        }

        public ICollection<User> GetUsers()
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id,Email, Password, Role, IsDelete from users";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            var users = new List<User>();
            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                users.Add(
                    new User
                    {
                        Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                        Email = reader["Email"].ToString() ?? "",
                        Password = reader["Password"].ToString() ?? "",
                        Role = (Role)Enum.Parse(typeof(Role), reader["Role"].ToString() ?? ""),
                        IsDelete = Convert.ToBoolean(reader["IsDelete"])
                    }
                );
            }
            return users;
        }

        public void Persist(User user)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = @"insert into users(
                Id,Email,Password,Role,IsDelete) values(@Id,@Email,@Password,@Role,@IsDelete)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", user.Id);
            sqlCommand.Parameters.AddWithValue("@Email", user.Email);
            sqlCommand.Parameters.AddWithValue("@Password", user.Password);
            sqlCommand.Parameters.AddWithValue("@Role", user.Role.ToString());
            sqlCommand.Parameters.AddWithValue("@IsDelete", user.IsDelete.ToString());
            sqlCommand.ExecuteNonQuery();
        }

        public bool CheckIfPasswordExist(string password)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Password from users where (Password = @Password)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Password", password);
            var reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                return true;
            }
            return false;
        }

        public void Update(User user)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = @"update users set Email=@Email,Password=@Password where Id=@Id";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", user.Id);
            sqlCommand.Parameters.AddWithValue("@Email", user.Email);
            sqlCommand.Parameters.AddWithValue("@Password", user.Password);
            sqlCommand.ExecuteNonQuery();
        }
    }
}