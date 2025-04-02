using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaPostManager.Models.Enum;

namespace SocialMediaPostManager.Models.Entities
{
    public class SocialMediaUser : BaseEntity
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}