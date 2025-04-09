using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaPostManager.Models.Entities;

namespace SocialMediaPostManager.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        void Persist(Comment comment);
        bool CheckComment(Guid id);
        void Update(Comment comment);
        void Delete(Guid id);
        Comment? GetComment(Guid id);
        ICollection<Comment> GetComments(Guid id);
        ICollection<Comment> GetComments();
        int GetCommentCount(Guid id);
    }
}