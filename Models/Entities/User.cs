using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaPostManager.Models.Enum;

namespace SocialMediaPostManager.Models.Entities
{
    public class User : BaseEntity
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public Role Role {get;set;}
    }
}