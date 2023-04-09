using CI_Platform.Entity.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entity.ViewModels
{
    public class MissionLandingModel
    {
        public int id { get; set; }
        public List<City> City { get; set; }
        public List<Country> Country { get; set; }
        public List<MissionTheme> MissionThemes { get; set; }
        public List<MissionSkill> MissionSkills { get; set; }
        public List<MissionRating> MissionRatings { get; set; }
        public List<Story> stories { get; set; }
        public List<Mission> RelatedMissions { get; set; }
        public List<Comment> Comments { get; set; }

        public List<MissionApplication> MissionApplications { get; set; }
        public string GoalText { get; set; }
        public List<FavoriteMission> FavMissionData { get; set; }
        public List<Mission> Missions { get; set; }
        public List<GoalMission> GoalMission { get; set; }
        public List<User> Users { get; set; }

        public bool IsFavorite { get; set; }

        public int? seats_left { get; set; }

        public bool? userApplied { get; set; }

        public bool approvalStatus { get; set; }
    }
}
