using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialMediaPostManager.Models.Enum;

namespace SocialMediaPostManager.Dtos
{
    public class SocialMediaUserDto
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }

    public class EditSocialMediaUserRequestModel
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public string? Address { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }

    public class RegisterSocialMediaUserRequestModel
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public Role Role { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}