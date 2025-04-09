using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SocialMediaPostManager.Context;
using SocialMediaPostManager.Models.Entities;
using SocialMediaPostManager.Models.Enum;
using SocialMediaPostManager.Repositories.Interfaces;

namespace SocialMediaPostManager.Repositories.Implenentations
{
    public class SocialMediaUserRepository : ISocialMediaUserRepository
    {
        private readonly SociaMediapostManagerContext _sociaMediapostManagerContext;

        public SocialMediaUserRepository()
        {
            _sociaMediapostManagerContext = new SociaMediapostManagerContext();
        }

        public bool CheckIfUserNameExist(string userName)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select UserName from SocialMediaUsers where UserName = @UserName";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@UserName", userName);
            var reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                return true;
            }
            return false;
        }

        public SocialMediaUser? GetMediaUser(string email)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id,Email,FirstName,LastName,UserName,Address,Gender,IsDelete,DateOfBirth from SocialMediaUsers where(Email = @Email)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Email", email);
            var reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                return new SocialMediaUser
                {
                    Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                    Email = reader["Email"].ToString() ?? "",
                    FirstName = reader["FirstName"].ToString() ?? "",
                    LastName = reader["LastName"].ToString() ?? "",
                    UserName = reader["UserName"].ToString() ?? "",
                    Address = reader["Address"].ToString() ?? "",
                    Gender = (Gender)Enum.Parse(typeof(Gender), reader["Gender"].ToString() ?? ""),
                    DateOfBirth = DateOnly.FromDateTime(DateTime.Parse(reader["DateOfBirth"].ToString() ?? "")),
                    IsDelete = Convert.ToBoolean(reader["IsDelete"])
                };
            }
            return null;
        }

        public SocialMediaUser? GetMediaUser(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id,Email,FirstName,UserName,LastName,Address,Gender,IsDelete,DateOfBirth from SocialMediaUsers where Id = @Id";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            var reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                return new SocialMediaUser
                {
                    Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                    Email = reader["Email"].ToString() ?? "",
                    FirstName = reader["FirstName"].ToString() ?? "",
                    LastName = reader["LastName"].ToString() ?? "",
                    UserName = reader["UserName"].ToString() ?? "",
                    Address = reader["Address"].ToString() ?? "",
                    Gender = (Gender)Enum.Parse(typeof(Gender), reader["Gender"].ToString() ?? ""),
                    DateOfBirth = DateOnly.FromDateTime(DateTime.Parse(reader["DateOfBirth"].ToString() ?? "")),
                    IsDelete = Convert.ToBoolean(reader["IsDelete"])
                };
            }
            return null;
        }

        public ICollection<SocialMediaUser> GetMediaUsers()
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select * from SocialMediaUsers";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            var mediaUsers = new List<SocialMediaUser>();
            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                mediaUsers.Add(
                    new SocialMediaUser
                    {
                        Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                        Email = reader["Email"].ToString() ?? "",
                        FirstName = reader["FirstName"].ToString() ?? "",
                        LastName = reader["LastName"].ToString() ?? "",
                        UserName = reader["UserName"].ToString() ?? "",
                        Address = reader["Address"].ToString() ?? "",
                        Gender = (Gender)Enum.Parse(typeof(Gender), reader["Gender"].ToString() ?? ""),
                        DateOfBirth = DateOnly.FromDateTime(DateTime.Parse(reader["DateOfBirth"].ToString() ?? "")),
                        IsDelete = bool.Parse(reader["IsDelete"].ToString() ?? "")
                    }
                );
            }
            return mediaUsers;
        }

        public void Persist(SocialMediaUser mediaUser)
        {
            using (var connection = _sociaMediapostManagerContext.OpenConnection())
            {
                connection.Open();
                var query = @"insert into SocialMediaUsers(
                Id,Email,FirstName,LastName,UserName,Gender,Address,DateOfBirth,IsDelete)
                values(@Id,@Email,@FirstName,@LastName,@UserName,@Gender,@Address,@DateOfBirth,@IsDelete)";
                MySqlCommand sqlCommand = new MySqlCommand(query, connection);
                sqlCommand.Parameters.AddWithValue("@Id", mediaUser.Id);
                sqlCommand.Parameters.AddWithValue("@Email", mediaUser.Email.ToLower());
                sqlCommand.Parameters.AddWithValue("@FirstName", mediaUser.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", mediaUser.LastName);
                sqlCommand.Parameters.AddWithValue("@UserName", mediaUser.UserName);
                sqlCommand.Parameters.AddWithValue("@Gender", mediaUser.Gender.ToString());
                sqlCommand.Parameters.AddWithValue("@Address", mediaUser.Address);
                sqlCommand.Parameters.AddWithValue("@DateOfBirth", mediaUser.DateOfBirth.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@IsDelete", mediaUser.IsDelete.ToString());
                sqlCommand.ExecuteNonQuery();
            }
        }

        public void Update(SocialMediaUser mediaUser)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = @"update SocialMediaUsers set Email = @Email,FirstName=@FirstName,LastName=@LastName,UserName=@UserName,Gender=@Gender,Address=@Address,DateOfBirth=@DateOfBirth where Id=@Id";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", mediaUser.Id);
            sqlCommand.Parameters.AddWithValue("@Email", mediaUser.Email);
            sqlCommand.Parameters.AddWithValue("@FirstName", mediaUser.FirstName);
            sqlCommand.Parameters.AddWithValue("@LastName", mediaUser.LastName);
            sqlCommand.Parameters.AddWithValue("@UserName", mediaUser.UserName);
            sqlCommand.Parameters.AddWithValue("@Gender", mediaUser.Gender);
            sqlCommand.Parameters.AddWithValue("@Address", mediaUser.Address);
            sqlCommand.Parameters.AddWithValue("@DateOfBirth", mediaUser.DateOfBirth.ToString("yyyy-MM-dd"));
            sqlCommand.ExecuteNonQuery();
        }

        public void SoftDelete(string email)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = @"update SocialMediaUsers set IsDelete = @IsDelete where Email=@Email";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Email", email);
            sqlCommand.Parameters.AddWithValue("@IsDelete", true.ToString());
            sqlCommand.ExecuteNonQuery();
        }
    }
}