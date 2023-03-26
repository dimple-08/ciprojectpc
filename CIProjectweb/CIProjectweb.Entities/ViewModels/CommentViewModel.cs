using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class CommentViewModel
    {
        public long CommentId { get; set; }

        public long UserId { get; set; }

        public long MissionId { get; set; }

        public string ApprovalStatus { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string? Avatar { get; set; }

        public DateTime? UpadetdAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string? CommentText { get; set; }

       public String? UserName { get; set; }

        
    }
}
