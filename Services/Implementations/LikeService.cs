using SocialMediaPostManager.Dtos;
using SocialMediaPostManager.Models.Entities;
using SocialMediaPostManager.Repositories.Implenentations;
using SocialMediaPostManager.Repositories.Interfaces;
using SocialMediaPostManager.Services.Interfaces;

namespace SocialMediaPostManager.Services.Implementations
{
    public class LikeService : ILikeService
    {
        private readonly IPostRepository _postRepository;
        private readonly ILikeRepository _likeRepository;

        public LikeService()
        {
            _postRepository = new PostRepository();
            _likeRepository = new LikeRepository();
        }

        public Result<string> LikePost(MakeLikeRequestModel makeLike)
        {
            var post = _postRepository.GetPost(makeLike.PostId);

            if (post == null)
            {
                return new Result<string>
                {
                    Data = null,
                    Message = "Post not found",
                    Status = false
                };
            }

            var isLike = _likeRepository.GetLikes().Any(like => like.PostId == makeLike.PostId && like.CreatedBy == makeLike.CreatedBy);
            if (isLike)
            {
                return new Result<string>
                {
                    Data = null,
                    Message = "Post has already liked",
                    Status = false
                };
            }
            var like = new Like
            {
                CreatedBy = makeLike.CreatedBy,
                PostId = makeLike.PostId,
            };
            _likeRepository.Persist(like);
            return new Result<string>
            {
                Data = like.Id.ToString(),
                Message = $"You have succesfully like post with Id: {like.PostId}",
                Status = false
            };
        }

        public ICollection<LikeDto> ViewAllLikes(Guid id)
        {
            List<LikeDto> likes = [];
            foreach (var like in _likeRepository.GetLikes(id))
            {
                var _like = new LikeDto
                {
                    CreatedBy = like.CreatedBy,
                    Id = like.Id,
                    DateCreated = like.DateCreated
                };
                likes.Add(_like);
            }
            return likes;
        }

        public ICollection<LikeDto> ViewAllLikes()
        {
            List<LikeDto> likes = [];
            foreach (var like in _likeRepository.GetLikes())
            {
                var _like = new LikeDto
                {
                    CreatedBy = like.CreatedBy,
                    Id = like.Id,
                    DateCreated = like.DateCreated
                };
                likes.Add(_like);
            }
            return likes;
        }
    }
}