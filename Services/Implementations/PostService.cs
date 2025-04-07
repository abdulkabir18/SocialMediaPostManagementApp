using SocialMediaPostManager.Dtos;
using SocialMediaPostManager.Models.Entities;
using SocialMediaPostManager.Repositories.Implenentations;
using SocialMediaPostManager.Repositories.Interfaces;
using SocialMediaPostManager.Services.Interfaces;

namespace SocialMediaPostManager.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ISocialMediaUserRepository _socialMediaUserRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly ICommentRepository _commentRepository;

        public PostService()
        {
            _postRepository = new PostRepository();
            _socialMediaUserRepository = new SocialMediaUserRepository();
            _likeRepository = new LikeRepository();
            _commentRepository = new CommentRepository();
        }

        public Result<string> EditPost(EditPostRequestModel editPost)
        {
            var post = _postRepository.GetPost(editPost.Id);
            if (post == null)
            {
                return new Result<string>
                {
                    Data = null,
                    Message = "Error occour",
                    Status = false
                };
            }

            post.Content = editPost.Content;
            post.Title = editPost.Title;
            _postRepository.Update(post);
            return new Result<string>
            {
                Data = post.Id.ToString(),
                Message = "Post updated succesfully",
                Status = true
            };
        }

        public Result<string> MakePost(CreatePostRequestModel createPost)
        {
            var mediaUser = _socialMediaUserRepository.GetMediaUser(createPost.SocialMediaUserId);
            if (mediaUser == null)
            {
                return new Result<string>
                {
                    Data = null,
                    Message = "Error occour",
                    Status = false
                };
            }

            var post = new Post
            {
                Content = createPost.Content,
                Title = createPost.Title,
                SocialMediaUserId = createPost.SocialMediaUserId,
                CreatedBy = $"{mediaUser.FirstName} {mediaUser.LastName}"
            };
            _postRepository.Persist(post);
            return new Result<string>
            {
                Data = post.Id.ToString(),
                Message = "Post created succesfully",
                Status = true
            };
        }

        public ICollection<PostDto> ViewAllPosts()
        {
            List<PostDto> posts = [];
            foreach (Post post in _postRepository.GetPosts())
            {
                var _post = new PostDto
                {
                    Content = post.Content,
                    CommentCount = _commentRepository.GetCommentCount(post.Id),
                    LikeCount = _likeRepository.GetLikeCount(post.Id),
                    CreatedBy = post.CreatedBy,
                    DateCreated = post.DateCreated,
                    Id = post.Id,
                    SocialMediaUserId = post.SocialMediaUserId,
                    IsDelete = post.IsDelete,
                    Title = post.Title
                };
                posts.Add(_post);
            }
            return posts;
        }

        public ICollection<PostDto> ViewAllPosts(Guid id)
        {
            List<PostDto> posts = [];
            foreach (Post post in _postRepository.GetPosts(id))
            {
                var _post = new PostDto
                {
                    Content = post.Content,
                    CommentCount = _commentRepository.GetCommentCount(post.Id),
                    LikeCount = _likeRepository.GetLikeCount(post.Id),
                    CreatedBy = post.CreatedBy,
                    DateCreated = post.DateCreated,
                    Id = post.Id,
                    SocialMediaUserId = post.SocialMediaUserId,
                    IsDelete = post.IsDelete,
                    Title = post.Title
                };
                posts.Add(_post);
            }
            return posts;
        }

        public ICollection<PostDto> ViewOthersPosts(Guid id)
        {
            List<PostDto> posts = [];
            foreach (Post post in _postRepository.GetOthersPosts(id))
            {
                var _post = new PostDto
                {
                    Content = post.Content,
                    CommentCount = _commentRepository.GetCommentCount(post.Id),
                    LikeCount = _likeRepository.GetLikeCount(post.Id),
                    CreatedBy = post.CreatedBy,
                    DateCreated = post.DateCreated,
                    Id = post.Id,
                    SocialMediaUserId = post.SocialMediaUserId,
                    IsDelete = post.IsDelete,
                    Title = post.Title
                };
                posts.Add(_post);
            }
            return posts;
        }

        public PostDto? ViewPost(Guid id)
        {
            var post = _postRepository.GetPost(id);
            if (post != null)
            {
                return new PostDto
                {
                    Content = post.Content,
                    CommentCount = _commentRepository.GetCommentCount(post.Id),
                    LikeCount = _likeRepository.GetLikeCount(post.Id),
                    CreatedBy = post.CreatedBy,
                    DateCreated = post.DateCreated,
                    Id = post.Id,
                    SocialMediaUserId = post.SocialMediaUserId,
                    IsDelete = post.IsDelete,
                    Title = post.Title
                };
            }
            return null;
        }
    }
}