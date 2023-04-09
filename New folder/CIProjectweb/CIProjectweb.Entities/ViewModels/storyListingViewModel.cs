using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class storyListingViewModel
    {
        public long StoryId { get; set; }

        public long UserId { get; set; }

        public long MissionId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? PublichedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public List<StoryMedium> Media { get; set; }
       public string? Theme { get; set; }
        public string? why_i_volunteer { get; set; }
        public string? image { get; set; }
        public string? Avtar { get; set; }
        public string? UserName { get; set; }
        public List<String?> MediaPaths { get; set; }
        public long Views { get; set; }
    }
}
