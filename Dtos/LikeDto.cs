using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaPostManager.Dtos
{
    public class LikeDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class MakeLikeRequestModel
    {
        public string? CreatedBy { get; set; }
        public Guid PostId { get; set; }
    }
}