using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class UserViewModel
    {
        public long UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Avatar { get; set; }

        public string? WhyIVolunteer { get; set; }

        public string? EmployeeId { get; set; }

        public string? Department { get; set; }

        public long? CityId { get; set; }

        public long? CountryId { get; set; }

        public string? ProfileText { get; set; }

        public string? LinkedInUrl { get; set; }

        public string? Title { get; set; }

        public bool Status { get; set; }

        public DateTime? CreatedAt { get; set; } = null!;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long PhoneNumber { get; set; }

        public virtual City? City { get; set; }

        public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

        public virtual Country? Country { get; set; }

        public virtual ICollection<FavouriteMission> FavouriteMissions { get; } = new List<FavouriteMission>();

        public virtual ICollection<MissionApplication> MissionApplications { get; } = new List<MissionApplication>();

        public virtual ICollection<MissionInvite> MissionInviteFromUsers { get; } = new List<MissionInvite>();

        public virtual ICollection<MissionInvite> MissionInviteToUsers { get; } = new List<MissionInvite>();

        public virtual ICollection<MissionRating> MissionRatings { get; } = new List<MissionRating>();

        public virtual ICollection<Story> StoryMissions { get; } = new List<Story>();

        public virtual ICollection<Story> StoryUsers { get; } = new List<Story>();

        public virtual ICollection<Timesheet> Timesheets { get; } = new List<Timesheet>();

        public virtual ICollection<UserSkill> UserSkills { get; } = new List<UserSkill>();
    }
}
