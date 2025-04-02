using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaPostManager.Models.Entities;

namespace SocialMediaPostManager.Repositories.Interfaces
{
    public interface ISocialMediaUserRepository
    {
        void Persist(SocialMediaUser mediaUser);
        void Update(SocialMediaUser mediaUser);
        SocialMediaUser? GetMediaUser(string userName);
        bool CheckIfUserNameExist(string userName);
        SocialMediaUser? GetMediaUser(Guid id);
        ICollection<SocialMediaUser> GetMediaUsers();
    }
}