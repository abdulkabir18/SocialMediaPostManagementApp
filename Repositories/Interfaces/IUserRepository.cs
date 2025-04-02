using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaPostManager.Models.Entities;

namespace SocialMediaPostManager.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void Persist(User user);
        void Update(User user);
        User? GetUser(string email, string password);
        bool CheckIfEmailExist(string email);
        bool CheckIfPasswordExist(string password);
        ICollection<User> GetUsers();
    }
}