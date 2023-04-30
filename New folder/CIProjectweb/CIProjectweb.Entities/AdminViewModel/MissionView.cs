using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.AdminViewModel
{
    public class MissionView
    {
        public List<Mission> missions = new List<Mission>();
        public List<MissionTheme> missionThemes= new List<MissionTheme>();
        public List<Country> countries= new List<Country>();
        public List<City> citys= new List<City>();
        public List<Skill> skills= new List<Skill>();
        public long MissionId { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public long ThemeId { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public long CityId { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public long CountryId { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        public string ShortDescription { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string MissionType { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? OrganizationName { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? GoalObjectiveText { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public int GoalValue { get; set; }
        public string? OrganizationDetalis { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public List<MissionSkill>? missionSkill;
        [Required(ErrorMessage = "Field can't be empty")]
        public List<MissionMedium>? missionMedia;
        [Required(ErrorMessage = "Field can't be empty")]
        public List<MissionDocument>? missionDocuments;

        public List<GoalMission>? goalmissions;
        [Required(ErrorMessage = "Field can't be empty")]
        public string? Availability { get; set; }
        public int? SeatsAvailable { get; set; }

        public DateTime? Deadline { get; set; }
        public long[] SkillId { get; set; } = null!;
    }
}
