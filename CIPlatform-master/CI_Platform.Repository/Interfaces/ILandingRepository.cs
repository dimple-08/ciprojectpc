using CI_Platform.Entity.DataModels;
using CI_Platform.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interfaces
{
    public interface ILandingRepository 
    {
        public MissionLandingModel LandingPageGet();
        public MissionLandingModel LandingPagePost(string[]? country, string[]? city, string[]? theme, string? searchTerm, string? sortValue, int pg);
        public bool AddToFav(long MissionId, long UserId);
        public bool RecomandUser(string EmailId, int MissionId);

        public VolunteeringMissionModel MissionVolunteering(int MissionId, long UserId);
        public VolunteeringMissionModel PostComment(long MissionId, long UserId, string CommentText);
        public MissionRating AddRating(int rating, long Id, long missionId);
        public bool ApplyNow(long missionId, long userId);
    }
}
