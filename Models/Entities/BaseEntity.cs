using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaPostManager.Models.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public bool IsDelete { get; set; }
        public DateTime DateCreated { get; init; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; } // who does the creation, like and comment
    }
}