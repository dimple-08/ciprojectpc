using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class MissionViewModel
    {
        public long MissionId { get; set; }

        public long ThemeId { get; set; }

        public long CityId { get; set; }

        public long CountryId { get; set; }

        public string Title { get; set; } = null!;

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MissionType { get; set; } = null!;

        public string? Theme  { get; set; }

        public bool? Status { get; set; }

        public string? OrganizationName { get; set; }

        public string? OrganizationDetail { get; set; }

        public string? Availability { get; set; }

        public DateTime? CreatedAt { get; set; } = null!;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string  City { get; set; } = null!;

        public string Country { get; set; } = null!;
        public string? MediaPath { get; set; }
        public MissionCardModel? missionCardModel { get; set; }
        public List<MissionMedium>? Media { get; set; }
        public List<String?> MediaPaths { get; set; }

        public List<String?> UserNames { get; set; }
        public string? SeatAvailable { get; set; }
        public bool isFavrouite { get; set; }
        public int UserPrevRating { get; set; }
        public bool isApplied { get; set; }
        public int ratingCount { get; set; }
        public bool isPending { get; set; }
        public float? GoalValue { get; set; }
        public string? GoalObjectiveText { get; set; }
        public int Rating { get; set; }
        public float? avgRating { get; set; }
        public float? progressBar { get; set; }
        public DateTime? deadline { get; set; }

        public List<String?> skill { get; set; }

        public int? alreadyVolunteered { get; set; }
    }
}
