using MySql.Data.MySqlClient;
using SocialMediaPostManager.Context;
using SocialMediaPostManager.Models.Entities;
using SocialMediaPostManager.Repositories.Interfaces;

namespace SocialMediaPostManager.Repositories.Implenentations
{
    public class LikeRepository : ILikeRepository
    {
        private readonly SociaMediapostManagerContext _sociaMediapostManagerContext;

        public LikeRepository()
        {
            _sociaMediapostManagerContext = new SociaMediapostManagerContext();
        }

        public int GetLikeCount(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select count(PostId) from likes where PostId = @PostId";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@PostId", id);
            var result = sqlCommand.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        public ICollection<Like> GetLikes(Guid id)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id,DateLiked,LikedBy from likes where (PostId = @PostId)";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@PostId", id);
            var likes = new List<Like>();
            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                likes.Add(
                    new Like
                    {
                        Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                        CreatedBy = reader["LikedBy"].ToString(),
                        DateCreated = DateTime.Parse(reader["DateLiked"].ToString() ?? "")
                    }
                );
            }
            return likes;
        }

        public ICollection<Like> GetLikes()
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = "select Id,DateLiked,LikedBy,PostId from likes";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            var likes = new List<Like>();
            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                likes.Add(
                    new Like
                    {
                        Id = Guid.Parse(reader["Id"].ToString() ?? ""),
                        CreatedBy = reader["LikedBy"].ToString(),
                        DateCreated = DateTime.Parse(reader["DateLiked"].ToString() ?? ""),
                        PostId = Guid.Parse(reader["PostId"].ToString() ?? "")
                    }
                );
            }
            return likes;
        }

        public void Persist(Like like)
        {
            using var connection = _sociaMediapostManagerContext.OpenConnection();
            connection.Open();
            var query = @"insert into likes(Id,PostId,DateLiked,LikedBy,IsDelete) 
                values(@Id,@PostId,@DateLiked,@LikedBy,@IsDelete);";
            MySqlCommand sqlCommand = new MySqlCommand(query, connection);
            sqlCommand.Parameters.AddWithValue("@Id", like.Id);
            sqlCommand.Parameters.AddWithValue("@PostId", like.PostId);
            sqlCommand.Parameters.AddWithValue("@DateLiked", like.DateCreated.ToString());
            sqlCommand.Parameters.AddWithValue("@LikedBy", like.CreatedBy);
            sqlCommand.Parameters.AddWithValue("@IsDelete", like.IsDelete.ToString());
            sqlCommand.ExecuteNonQuery();
        }
    }
}