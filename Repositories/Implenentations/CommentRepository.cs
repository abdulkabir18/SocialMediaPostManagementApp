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
    public class CommentRepository : ICommentRepository
    {
        private readonly SociaMediapostManagerContext _sociaMediapostManagerContext;

        public CommentRepository()
        {
            _sociaMediapostManagerContext = new SociaMediapostManagerContext();
        }

        public int GetCommentCount(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select count(PostId) from comments where PostId = @PostId";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@PostId", id);
            var result = sqlCommand.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public Comment? GetComment(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id,DateComment, CommentBy, Message from comments where (Id = @Id)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            using var reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                return new Comment
                {
                    Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                    CreatedBy = reader["CommentBy"].ToString(),
                    DateCreated = DateTime.Parse(reader["DateComment"].ToString() ?? ""),
                    Message = reader["Message"].ToString() ?? ""
                };
            }
            return null;
        }

        public ICollection<Comment> GetComments(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id,DateComment, CommentBy, Message from comments where (PostId = @PostId)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@PostId", id);
            var comments = new List<Comment>();
            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                comments.Add(
                    new Comment
                    {
                        Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                        CreatedBy = reader["CommentBy"].ToString(),
                        DateCreated = DateTime.Parse(reader["DateComment"].ToString() ?? ""),
                        Message = reader["Message"].ToString() ?? ""
                    }
                );
            }
            return comments;
        }

        public ICollection<Comment> GetComments()
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id,DateComment, CommentBy, Message from comments";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            var comments = new List<Comment>();
            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                comments.Add(
                    new Comment
                    {
                        Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                        CreatedBy = reader["CommentBy"].ToString(),
                        DateCreated = DateTime.Parse(reader["DateComment"].ToString() ?? ""),
                        Message = reader["Message"].ToString() ?? ""
                    }
                );
            }
            return comments;
        }

        public void Persist(Comment comment)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = @"insert into comments(Id,PostId,Message,DateComment,CommentBy,IsDelete) 
                values(@Id,@PostId,@Message,@DateComment,@CommentBy,@IsDelete)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", comment.Id);
            sqlCommand.Parameters.AddWithValue("@PostId", comment.PostId);
            sqlCommand.Parameters.AddWithValue("@Message", comment.Message);
            sqlCommand.Parameters.AddWithValue("@DateComment", comment.DateCreated.ToString());
            sqlCommand.Parameters.AddWithValue("@CommentBy", comment.CreatedBy?.ToString());
            sqlCommand.Parameters.AddWithValue("@IsDelete", comment.IsDelete.ToString());
            sqlCommand.ExecuteNonQuery();
        }

        public void Update(Comment comment)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = @"update comments set Message=@Message where Id=@Id";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", comment.Id);
            sqlCommand.Parameters.AddWithValue("@Message", comment.Message);
            sqlCommand.ExecuteNonQuery();
        }
    }
}