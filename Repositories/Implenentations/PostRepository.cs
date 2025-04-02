using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SocialMediaPostManager.Context;
using SocialMediaPostManager.Models.Entities;
using SocialMediaPostManager.Repositories.Interfaces;

namespace SocialMediaPostManager.Repositories.Implenentations
{
    public class PostRepository : IPostRepository
    {
        private readonly SociaMediapostManagerContext _sociaMediapostManagerContext;

        public PostRepository()
        {
            _sociaMediapostManagerContext = new SociaMediapostManagerContext();
        }

        public Post? GetPost(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select * from Posts where(Id = @Id)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            var reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                return new Post
                {
                    Content = reader["Content"].ToString() ?? "",
                    Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                    Title = reader["Title"].ToString(),
                    SocialMediaUserId = Guid.Parse(reader["SocialMediaUserId"].ToString() ?? ""),
                    CreatedBy = reader["CreatedBy"].ToString(),
                    DateCreated = DateTime.Parse(reader["DateCreated"].ToString() ?? ""),
                    IsDelete = Convert.ToBoolean(reader["IsDelete"])
                };
            }
            return null;
        }

        public ICollection<Post> GetPosts()
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select * from posts";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            var posts = new List<Post>();
            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                posts.Add(
                    new Post
                    {
                        Content = reader["Content"].ToString() ?? "",
                        Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                        Title = reader["Title"].ToString(),
                        SocialMediaUserId = Guid.Parse(reader["SocialMediaUserId"].ToString() ?? ""),
                        CreatedBy = reader["CreatedBy"].ToString(),
                        DateCreated = DateTime.Parse(reader["DateCreated"].ToString() ?? ""),
                        IsDelete = Convert.ToBoolean(reader["IsDelete"])
                    }
                );
            }
            return posts;
        }

        public ICollection<Post> GetPosts(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select * from posts where SocialMediaUserId = @Id";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            var posts = new List<Post>();
            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                posts.Add(
                    new Post
                    {
                        Content = reader["Content"].ToString() ?? "",
                        Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                        Title = reader["Title"].ToString(),
                        SocialMediaUserId = Guid.Parse(reader["SocialMediaUserId"].ToString() ?? ""),
                        CreatedBy = reader["CreatedBy"].ToString(),
                        DateCreated = DateTime.Parse(reader["DateCreated"].ToString() ?? ""),
                        IsDelete = Convert.ToBoolean(reader["IsDelete"])
                    }
                );
            }
            return posts;
        }

        public ICollection<Post> GetOthersPosts(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select * from posts where SocialMediaUserId != @Id";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            var posts = new List<Post>();
            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                posts.Add(
                    new Post
                    {
                        Content = reader["Content"].ToString() ?? "",
                        Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                        Title = reader["Title"].ToString(),
                        SocialMediaUserId = Guid.Parse(reader["SocialMediaUserId"].ToString() ?? ""),
                        CreatedBy = reader["CreatedBy"].ToString(),
                        DateCreated = DateTime.Parse(reader["DateCreated"].ToString() ?? ""),
                        IsDelete = Convert.ToBoolean(reader["IsDelete"])
                    }
                );
            }
            return posts;
        }

        public void Persist(Post post)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = @"insert into posts(Id,Title,Content,SocialMediaUserId,DateCreated,CreatedBy,LikeCount,CommentCount,IsDelete) 
                values(@Id,@Title,@Content,@SocialMediaUserId,@DateCreated,@CreatedBy,@IsDelete)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", post.Id);
            sqlCommand.Parameters.AddWithValue("@Title", post.Title);
            sqlCommand.Parameters.AddWithValue("@Content", post.Content);
            sqlCommand.Parameters.AddWithValue("@SocialMediaUserId", post.SocialMediaUserId);
            sqlCommand.Parameters.AddWithValue("@DateCreated", post.DateCreated.ToString());
            sqlCommand.Parameters.AddWithValue("@CreatedBy", post.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@IsDelete", post.IsDelete.ToString());
            sqlCommand.ExecuteNonQuery();
        }

        public void Update(Post post)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = @"update posts set Title = @Title, Content = @Content where Id = @Id";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", post.Id);
            sqlCommand.Parameters.AddWithValue("@Title", post.Title);
            sqlCommand.Parameters.AddWithValue("@Content", post.Content);
            sqlCommand.ExecuteNonQuery();
        }
    }
}