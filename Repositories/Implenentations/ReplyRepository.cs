using MySql.Data.MySqlClient;
using SocialMediaPostManager.Context;
using SocialMediaPostManager.Models.Entities;
using SocialMediaPostManager.Repositories.Interfaces;

namespace SocialMediaPostManager.Repositories.Implenentations
{
    public class ReplyRepository : IReplyRepository
    {
        private readonly SociaMediapostManagerContext _sociaMediapostManagerContext;

        public ReplyRepository()
        {
            _sociaMediapostManagerContext = new SociaMediapostManagerContext();
        }

        public ICollection<Reply> GetReplies(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id, DateReply, ReplyBy, MediaUserId, Message from replies where (CommentId = @CommentId)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@CommentId", id);
            using var reader = sqlCommand.ExecuteReader();
            var replies = new List<Reply>();
            while (reader.Read())
            {
                replies.Add(new Reply
                {
                    Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                    CreatedBy = reader["ReplyBy"].ToString(),
                    DateCreated = DateTime.Parse(reader["DateReply"].ToString() ?? ""),
                    Message = reader["Message"].ToString() ?? "",
                    MediaUserId = Guid.Parse(reader["MediaUserId"].ToString() ?? "")
                });
            }
            return replies;
        }

        public ICollection<Reply> GetReplies()
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id, DateReply, ReplyBy, MediaUserId, Message from replies";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            using var reader = sqlCommand.ExecuteReader();
            var replies = new List<Reply>();
            while (reader.Read())
            {
                replies.Add(new Reply
                {
                    Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                    CreatedBy = reader["ReplyBy"].ToString(),
                    DateCreated = DateTime.Parse(reader["DateReply"].ToString() ?? ""),
                    Message = reader["Message"].ToString() ?? "",
                    MediaUserId = Guid.Parse(reader["MediaUserId"].ToString() ?? "")
                });
            }
            return replies;
        }
        public Reply? GetReply(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id, DateReply, ReplyBy, MediaUserId, Message from replies where (CommentId = @CommentId)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@CommentId", id);
            using var reader = sqlCommand.ExecuteReader();
            if (reader.Read())
            {
                return new Reply
                {
                    Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                    CreatedBy = reader["ReplyBy"].ToString(),
                    DateCreated = DateTime.Parse(reader["DateReply"].ToString() ?? ""),
                    Message = reader["Message"].ToString() ?? "",
                    MediaUserId = Guid.Parse(reader["MediaUserId"].ToString() ?? "")
                };
            }
            return null;
        }

        public int GetReplyCount(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select count(CommentId) from replies where CommentId = @CommentId";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@CommentId", id);
            var result = sqlCommand.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public void Persist(Reply reply)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "insert into replies (Id, CommentId, Message, DateReply, ReplyBy, MediaUserId) values (@Id, @CommentId, @Message, @DateReply, @ReplyBy, @MediaUserId)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", reply.Id);
            command.Parameters.AddWithValue("@CommentId", reply.CommentId);
            command.Parameters.AddWithValue("@Message", reply.Message);
            command.Parameters.AddWithValue("@DateReply", reply.DateCreated);
            command.Parameters.AddWithValue("@ReplyBy", reply.CreatedBy);
            command.Parameters.AddWithValue("@MediaUserId", reply.MediaUserId);
            command.ExecuteNonQuery();
        }

        public void Delete(Guid commentId)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "delete from replies where CommentId = @CommentId";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@CommentId", commentId);
            sqlCommand.ExecuteNonQuery();
        }

        // public void Update(Reply reply)
        // {
        //     throw new NotImplementedException();
        // }
    }
}