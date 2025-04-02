using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaPostManager.Models.Entities
{
    public class Comment : BaseEntity
    {
        public Guid PostId {get;set;}
        public required string Message{get;set;}
    }
}