using CIProjectweb.Entities.DataModels;
using CIProjectweb.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Repository.Repository.Interface
{
    public  interface IUserInterface
    {
        public bool AddUser(RegistrationViewModel objuservm);

        public bool ValideUserEmail(ForgotPasswordViewModel objFpvm,string token);

        public bool ResetPassword(string email, string token);

        public bool updatePassword(ResetPAsswordViewModel objreset);

        public LandingPAgeViewModel GetCustomers(int currentPage, int u_id);

        public List<Mission> getmission(string missionName);

        public List<MissionViewModel> getmissions(int u_id);

        public List<MissionViewModel> getmissions();


        public MissionViewModel getmission(int id);

        public List<CountryViewModel> CountryList();

        public List<CityViewModel> CityList();

        public List<MissionViewModel> getrelatedmissions(int id, int u_id);

        public List<ThemeViewModel> ThemeList();

        public MissionRating Rating(int id, long missionId);

        public FavouriteMission FavouriteMission(int id, long missionId);

        public bool Update_favourite(FavouriteMission favoriteMission, long missionId, int u_id);

        public MissionViewModel getmission(int id, int u_id);

        public MissionRating ADD_Rating(MissionRating ratingExists, string rating, int u_id, long missionId);

        public bool Update_Rating(MissionRating ratingExists, string rating, int u_id, long missionId);

        public bool Add_Comment(string commentText, int missionId, int u_id);

        public List<CommentViewModel> getcomment(int id);

        public User getuserEmail(string mail);
        public List<MissionDocument> getmissionDocument(int missionid);
        public ShareStoryViewModel getsharestory();
        public bool alreadystory(int MissionId, int UserId);
        public void story(string[] Image, int MissionId, string Title, DateTime Date, string Description, int UserId, string[] videoUrl);
        public MissionInvite missionInviteExists(int fromUserid, int ToUserId, long missionId);
        public bool ADDMissionInvite(MissionInvite InviteExixts, int fomUserId, int ToUserId, long missionId);
    }
}
