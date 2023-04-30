using CIProjectweb.Entities.DataModels;
using CIProjectweb.Entities.ViewModels;
using CIProjectweb.Repository.Repository.Interface;

using System.Net;
using System.Net.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Microsoft.AspNetCore.Http;



namespace CIProjectweb.Repository.Repository
{
    public class UserInterface:IUserInterface
    {
        private readonly CIDbContext _objdb;

       
       
        public UserInterface(CIDbContext objdb)
        {
            _objdb = objdb;
           
        }
        public bool AddUser(RegistrationViewModel objuser)
        {
           var userexsists= _objdb.Users.Where(a => a.Email.Equals(objuser.Email)&& a.DeletedAt==null && a.Status==true).FirstOrDefault();
            if (userexsists == null)
            {
                var user = new User()
                {
                    FirstName = objuser.FirstName,
                    LastName = objuser.LastName,
                    PhoneNumber = objuser.PhoneNumber,
                    Email = objuser.Email,
                    Password = objuser.Password,

                };
                _objdb.Users.Add(user);
                _objdb.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ValideUserEmail(ForgotPasswordViewModel objFpvm,string token)
        {
            var userexsists = _objdb.Users.Where(a => a.Email.Equals(objFpvm.Email) && a.DeletedAt == null && a.Status == true).FirstOrDefault();
            if (userexsists != null)
            {
                
                // Store the token in the password resets table with the user's email
                var passwordReset_ = new PasswordReset()
                {
                    Email = objFpvm.Email,
                    Token = token
                };
                _objdb.PasswordResets.Add(passwordReset_);
                _objdb.SaveChanges();


                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ResetPassword(string email,string token)
        {
            var userexsists = _objdb.PasswordResets.Where(a => a.Email.Equals(email)&&a.Token.Equals(token)).FirstOrDefault();
            
            if (userexsists == null)
            {
                return false;
            }
            else
            {
              
                return true;
            }

        }

        public bool updatePassword(ResetPAsswordViewModel objreset)
        {
            var userexsists = _objdb.Users.Where(a => a.Email.Equals(objreset.Email) && a.DeletedAt == null && a.Status == true).FirstOrDefault();
            if (userexsists == null)
            {
                return false;
            }
            else
            {
                userexsists.Password=objreset.Password; 
                _objdb.Users.Update(userexsists);
                _objdb.SaveChanges();
                return true;
            }
        }

        public LandingPAgeViewModel GetCustomers(int currentPage,int u_id)
        {
            int maxRows = 6;
            LandingPAgeViewModel customerModel = new LandingPAgeViewModel();

            customerModel.UsersList = _objdb.Users.Where(a => a.UserId == u_id).FirstOrDefault();

            customerModel.MissionList = _objdb.Missions.ToList();
            double pageCount = (double)((decimal)_objdb.Missions.Count() / Convert.ToDecimal(maxRows));
            customerModel.PageCount = (int)Math.Ceiling(pageCount);

            customerModel.CurrentPageIndex = currentPage;

            return customerModel;
        }

        public List<Mission> getmission(string missionName)
        {
            List<Mission> MissionList = (from mission in _objdb.Missions
                                  where mission.Title.Contains(missionName)
                                  select mission).ToList();
            return MissionList;
        }
        public List<CountryViewModel> CountryList()
        {
            List<Country> countryList = _objdb.Countries.ToList();
            List<CountryViewModel> countryViews = new List<CountryViewModel>();
            foreach (var country in countryList)
            {
                CountryViewModel countryview=new CountryViewModel();
               
                var countryname= _objdb.Countries.SingleOrDefault(c => c.CountryId == country.CountryId);
                countryview.CountryId = countryname.CountryId;
                countryview.Name = countryname.Name;
                countryViews.Add(countryview);

            }
            return countryViews;
        }
        public List<ThemeViewModel> ThemeList()
        {
            List<MissionTheme> ThemeList = _objdb.MissionThemes.ToList();
            List<ThemeViewModel> themeViews = new List<ThemeViewModel>();
            foreach (var cities in ThemeList)
            {
                ThemeViewModel themeview = new ThemeViewModel();
                
                var themename = _objdb.MissionThemes.SingleOrDefault(c => c.MissionThemeId == cities.MissionThemeId);
                themeview.Title = themename.Title;
                themeview.MissionThemeId = themename.MissionThemeId;
                themeViews.Add(themeview);

            }
            return themeViews;
        }
        public List<CityViewModel> CityList()
        {
            List<City> CityList = _objdb.Cities.ToList();
            List<CityViewModel> CityViews = new List<CityViewModel>();
            foreach (var cities in CityList)
            {
                CityViewModel cityview = new CityViewModel();
                
                var countryname = _objdb.Cities.SingleOrDefault(c => c.CityId == cities.CityId);
                cityview.CityId = cities.CityId;
                cityview.Name = countryname.Name;
                CityViews.Add(cityview);

            }
            return CityViews;
        }
        public List<MissionViewModel> getmissions(int u_id)
        {
           
            List<Mission> MissionList = _objdb.Missions.ToList();
            List<MissionViewModel> missionViews = new List<MissionViewModel>();
            foreach (var mission in MissionList)
            {
                MissionViewModel missionView = new MissionViewModel();
                missionView.Availability = mission.Availability;
                missionView.Title = mission.Title;
                var city = _objdb.Cities.SingleOrDefault(c => c.CityId == mission.CityId);
                if (city != null)
                {
                    missionView.City = city.Name;
                }

                var country = _objdb.Countries.SingleOrDefault(c => c.CountryId == mission.CountryId);
                if (country != null)
                {
                    missionView.Country = country.Name;
                }
                var Theme = _objdb.MissionThemes.SingleOrDefault(t => t.MissionThemeId == mission.ThemeId);
                if (Theme != null)
                {
                    missionView.Theme = Theme.Title;
                }
                var image = _objdb.MissionMedia.FirstOrDefault(t => t.MissionId == mission.MissionId);
                if (image != null)
                {
                    missionView.MediaPath = image.MediaPath;
                }

                var goalvalue = _objdb.GoalMissions.SingleOrDefault(t => t.MissionId == mission.MissionId);
                if (goalvalue != null)
                {
                    missionView.GoalValue = goalvalue.GoalValue;
                }
                var goaltext = _objdb.GoalMissions.SingleOrDefault(t => t.MissionId == mission.MissionId);
                if (goalvalue != null)
                {
                    missionView.GoalObjectiveText = goalvalue.GoalObjectiveText;
                }

                var favourite = _objdb.FavouriteMissions.Where(x => x.MissionId == mission.MissionId && x.UserId == u_id && x.DeletedAt == null).ToList();
                if (favourite.Count>0)
                {
                    missionView.isFavrouite = true;
                }
                else
                {
                    missionView.isFavrouite = false;
                }
                var ratings = _objdb.MissionRatings.Where(t => t.MissionId == mission.MissionId).ToList();
                var rating = 0;
                var sum = 0;
                foreach (var entry in ratings)
                {
                    sum = sum + int.Parse(entry.Rating);

                }
                rating = sum / ratings.Count;
                if (rating != null)
                {
                    missionView.Rating = rating;
                }
                missionView.deadline = mission.Deadline;
                missionView.Description = mission.Description;
                missionView.CityId = mission.CityId;
                missionView.CountryId = mission.CountryId;
                missionView.CreatedAt = mission.CreatedAt;
                missionView.UpdatedAt = mission.UpdatedAt;
                missionView.DeletedAt = mission.DeletedAt;
                missionView.ShortDescription = mission.ShortDescription;
                missionView.MissionId = mission.MissionId;
                missionView.StartDate = mission.StartDate;
                missionView.EndDate = mission.EndDate;
                missionView.SeatAvailable = mission.SeatAvailable;
                missionView.ThemeId = mission.ThemeId;
                missionView.Status = mission.Status;
                missionView.MissionType = mission.MissionType;
                missionView.OrganizationDetail = mission.OrganizationDetail;
                missionView.OrganizationName = mission.OrganizationName;
                missionViews.Add(missionView);
            }
            return missionViews;
        }

        public List<MissionViewModel> getmissions()
        {

            List<Mission> MissionList = _objdb.Missions.ToList();
            List<MissionViewModel> missionViews = new List<MissionViewModel>();
            foreach (var mission in MissionList)
            {
                MissionViewModel missionView = new MissionViewModel();
                missionView.Availability = mission.Availability;
                missionView.Title = mission.Title;
                var city = _objdb.Cities.SingleOrDefault(c => c.CityId == mission.CityId);
                if (city != null)
                {
                    missionView.City = city.Name;
                }

                var country = _objdb.Countries.SingleOrDefault(c => c.CountryId == mission.CountryId);
                if (country != null)
                {
                    missionView.Country = country.Name;
                }
                var Theme = _objdb.MissionThemes.SingleOrDefault(t => t.MissionThemeId == mission.ThemeId);
                if (Theme != null)
                {
                    missionView.Theme = Theme.Title;
                }
                var image = _objdb.MissionMedia.FirstOrDefault(t => t.MissionId == mission.MissionId);
                if (image != null)
                {
                    missionView.MediaPath = image.MediaPath;
                }
                var goalvalue = _objdb.GoalMissions.SingleOrDefault(t => t.MissionId == mission.MissionId);
                if (goalvalue != null)
                {
                    missionView.GoalValue = goalvalue.GoalValue;
                }
                var goaltext = _objdb.GoalMissions.SingleOrDefault(t => t.MissionId == mission.MissionId);
                if (goalvalue != null)
                {
                    missionView.GoalObjectiveText = goalvalue.GoalObjectiveText;
                }

                missionView.isFavrouite = false;
                
                var ratings = _objdb.MissionRatings.Where(t => t.MissionId == mission.MissionId).ToList();
                var rating = 0;
                var sum = 0;
                foreach (var entry in ratings)
                {
                    sum = sum + int.Parse(entry.Rating);

                }
                rating = sum / ratings.Count;
                if (rating != null)
                {
                    missionView.Rating = rating;
                }
                missionView.Description = mission.Description;
                missionView.CityId = mission.CityId;
                missionView.CountryId = mission.CountryId;
                missionView.CreatedAt = mission.CreatedAt;
                missionView.UpdatedAt = mission.UpdatedAt;
                missionView.DeletedAt = mission.DeletedAt;
                missionView.ShortDescription = mission.ShortDescription;
                missionView.MissionId = mission.MissionId;
                missionView.StartDate = mission.StartDate;
                missionView.EndDate = mission.EndDate;
                missionView.SeatAvailable = mission.SeatAvailable;
                missionView.ThemeId = mission.ThemeId;
                missionView.Status = mission.Status;
                missionView.MissionType = mission.MissionType;
                missionView.OrganizationDetail = mission.OrganizationDetail;
                missionView.OrganizationName = mission.OrganizationName;
                missionViews.Add(missionView);
            }
            return missionViews;
        }
      

        public MissionViewModel getmission(int id,int u_id)
        {

            Mission mission = (Mission)_objdb.Missions.Where(m=>m.MissionId==id).FirstOrDefault();
           
           
                MissionViewModel missionView = new MissionViewModel();
                missionView.Availability = mission.Availability == null? "Daily":mission.Availability;
                missionView.Title = mission.Title;
                var city = _objdb.Cities.SingleOrDefault(c => c.CityId == mission.CityId);
                if (city != null)
                {
                    missionView.City = city.Name;
                }

                var country = _objdb.Countries.SingleOrDefault(c => c.CountryId == mission.CountryId);
                if (country != null)
                {
                    missionView.Country = country.Name;
                }
                var Theme = _objdb.MissionThemes.SingleOrDefault(t => t.MissionThemeId == mission.ThemeId);
                if (Theme != null)
                {
                    missionView.Theme = Theme.Title;
                }
                var image = _objdb.MissionMedia.FirstOrDefault(t => t.MissionId == mission.MissionId);
                if (image != null)
                {
                    missionView.MediaPath = image.MediaPath;
                }
            var images = _objdb.MissionMedia.Where(t => t.MissionId == mission.MissionId).ToList();
            if (images != null)
            {
                var mediaPaths = new List<string>();
                foreach (var image1 in images)
                {
                    mediaPaths.Add(image1.MediaPath);
                }
                missionView.MediaPaths = mediaPaths;
            }
            var skill = _objdb.MissionSkills.Where(t => t.MissionId == mission.MissionId).ToList();
            if (skill!=null)
            {    var name=new List<string>();
                foreach (var skills in skill)
                {
                    var skill_name = _objdb.Skills.FirstOrDefault(t => t.SkillId == skills.SkillId);
                    if (skill_name != null)
                    {
                        name.Add(skill_name.SkillName);
                    }
                }
                missionView.skill = name;
                
            }
            var UserName = _objdb.MissionApplications.Where(t => t.MissionId == mission.MissionId).ToList();
            if (UserName != null)
            {

                var names = new List<string>();
                foreach (var image1 in UserName)
                {
                    var user = _objdb.Users.Where(t => t.UserId == image1.UserId).FirstOrDefault();
                    names.Add(user.FirstName);
                }
                missionView.UserNames = names;
            }
           
            var Applided = _objdb.MissionApplications.Where(e => e.MissionId == mission.MissionId && e.UserId == u_id ).ToList();
            var Pending = _objdb.MissionApplications.Where(e => e.MissionId == mission.MissionId && e.UserId == u_id && e.ApprovalStatus=="PENDING").ToList();
            if (Applided.Count > 0)
            {
                missionView.isApplied = true;
            }
            else
            {
                missionView.isApplied = false;
            }
            if (Pending.Count > 0)
            {
                missionView.isPending = true;
            }
            else
            {
                missionView.isPending = false;
            }
            var goalvalue = _objdb.GoalMissions.SingleOrDefault(t => t.MissionId == mission.MissionId);
            if (goalvalue != null)
            {
               float action = (float)(_objdb.Timesheets.Where(x => x.MissionId == mission.MissionId && x.DeletedAt == null).Select(x => x.Action).Sum());
            float totalGoal = goalvalue.GoalValue;
            missionView.progressBar = action * 100 / totalGoal;
                missionView.GoalValue = goalvalue.GoalValue;
            }
           missionView.Media=_objdb.MissionMedia.Where(m=>m.MissionId==mission.MissionId).ToList();
           
            var goaltext = _objdb.GoalMissions.SingleOrDefault(t => t.MissionId == mission.MissionId);
            if (goalvalue != null)
            {
                missionView.GoalObjectiveText = goalvalue.GoalObjectiveText;
            }
            var ratingUser = _objdb.MissionRatings.SingleOrDefault(t => t.MissionId == mission.MissionId && t.UserId == u_id);
            if (ratingUser!=null)
            {
                missionView.UserPrevRating = int.Parse(ratingUser.Rating);

            }
            else
            {
                missionView.UserPrevRating=0;
            }

            var favourite = _objdb.FavouriteMissions.Where(x => x.MissionId == mission.MissionId && x.UserId == u_id && x.DeletedAt == null).ToList();
            if (favourite.Count > 0)
            {
                missionView.isFavrouite = true;
            }
            else
            {
                missionView.isFavrouite = false;
            }

            var ratings = _objdb.MissionRatings.Where(t => t.MissionId == mission.MissionId).ToList();
            var count=ratings.Count;
            missionView.ratingCount= count;
                var rating = 0;
                var sum = 0;
                foreach (var entry in ratings)
                {
                    sum = sum + int.Parse(entry.Rating);

                }
            if (ratings.Count>0)
            {
                rating = sum / ratings.Count;
            }
            else
            {
                rating=5;
            }
                
                if (rating != null)
                {
                    missionView.Rating = rating;
                }

            var ratings1 = _objdb.MissionRatings.Where(t => t.MissionId == mission.MissionId).ToList();
           
            float rating1 = 0;
            float sum1 = 0;
            foreach (var entry in ratings1)
            {
                sum1 = sum1 + int.Parse(entry.Rating);

            }
            rating1 = sum1 / ratings1.Count;
            if (rating != null)
            {
                missionView.avgRating = rating1;
            }
            missionView.deadline = mission.Deadline;
            missionView.Description = mission.Description;
                missionView.CityId = mission.CityId;
                missionView.CountryId = mission.CountryId;
                missionView.CreatedAt = mission.CreatedAt;
                missionView.UpdatedAt = mission.UpdatedAt;
                missionView.DeletedAt = mission.DeletedAt;
                missionView.ShortDescription = mission.ShortDescription;
                missionView.MissionId = mission.MissionId;
                missionView.StartDate = mission.StartDate;
                missionView.EndDate = mission.EndDate;
            var seats = (int.Parse(mission.SeatAvailable) - (_objdb.MissionApplications.Where(x => x.MissionId == mission.MissionId && x.ApprovalStatus=="ACCEPT").Count()));
            
            if (int.Parse(mission.SeatAvailable) > 0)
            {
                missionView.SeatAvailable = seats.ToString();
            }
            else
            {
                missionView.SeatAvailable=mission.SeatAvailable;
            }
            missionView.alreadyVolunteered=(_objdb.MissionApplications.Where(x => x.MissionId == mission.MissionId && x.ApprovalStatus == "ACCEPT").Count());
            missionView.ThemeId = mission.ThemeId;
                missionView.Status = mission.Status;
                missionView.MissionType = mission.MissionType;
                missionView.OrganizationDetail = mission.OrganizationDetail;
                missionView.OrganizationName = mission.OrganizationName;
                
            
            return missionView;
        }

        public MissionViewModel getmission(int id)
        {

            Mission mission = (Mission)_objdb.Missions.Where(m => m.MissionId == id).FirstOrDefault();


            MissionViewModel missionView = new MissionViewModel();
            missionView.Availability = mission.Availability;
            missionView.Title = mission.Title;
            var city = _objdb.Cities.SingleOrDefault(c => c.CityId == mission.CityId);
            if (city != null)
            {
                missionView.City = city.Name;
            }

            var country = _objdb.Countries.SingleOrDefault(c => c.CountryId == mission.CountryId);
            if (country != null)
            {
                missionView.Country = country.Name;
            }
            var Theme = _objdb.MissionThemes.SingleOrDefault(t => t.MissionThemeId == mission.ThemeId);
            if (Theme != null)
            {
                missionView.Theme = Theme.Title;
            }
            var image = _objdb.MissionMedia.FirstOrDefault(t => t.MissionId == mission.MissionId);
            if (image != null)
            {
                missionView.MediaPath = image.MediaPath;
            }
            var images = _objdb.MissionMedia.Where(t => t.MissionId == mission.MissionId).ToList();
            if (images != null)
            {
                var mediaPaths = new List<string>();
                foreach (var image1 in images)
                {
                    mediaPaths.Add(image1.MediaPath);
                }
                missionView.MediaPaths = mediaPaths;
            }

            var UserName = _objdb.MissionApplications.Where(t => t.MissionId == mission.MissionId).ToList();
            if (UserName != null)
            {

                var names = new List<string>();
                foreach (var image1 in UserName)
                {
                    var user = _objdb.Users.Where(t => t.UserId == image1.UserId).FirstOrDefault();
                    names.Add(user.FirstName);
                }
                missionView.UserNames = names;
            }

           
            var goalvalue = _objdb.GoalMissions.SingleOrDefault(t => t.MissionId == mission.MissionId);
            if (goalvalue != null)
            {
                missionView.GoalValue = goalvalue.GoalValue;
            }
            var goaltext = _objdb.GoalMissions.SingleOrDefault(t => t.MissionId == mission.MissionId);
            if (goalvalue != null)
            {
                missionView.GoalObjectiveText = goalvalue.GoalObjectiveText;
            }

            missionView.isFavrouite = false;

            var ratings = _objdb.MissionRatings.Where(t => t.MissionId == mission.MissionId).ToList();
            var rating = 0;
            var sum = 0;
            foreach (var entry in ratings)
            {
                sum = sum + int.Parse(entry.Rating);

            }
            rating = sum / ratings.Count;
            if (rating != null)
            {
                missionView.Rating = rating;
            }
            var ratings1 = _objdb.MissionRatings.Where(t => t.MissionId == mission.MissionId).ToList();

            float rating1 = 0;
            float sum1 = 0;
            foreach (var entry in ratings1)
            {
                sum = sum + int.Parse(entry.Rating);

            }
            rating1 = sum / ratings1.Count;
            if (rating != null)
            {
                missionView.avgRating = rating1;
            }
            missionView.deadline = mission.Deadline;
            missionView.Description = mission.Description;
            missionView.CityId = mission.CityId;
            missionView.CountryId = mission.CountryId;
            missionView.CreatedAt = mission.CreatedAt;
            missionView.UpdatedAt = mission.UpdatedAt;
            missionView.DeletedAt = mission.DeletedAt;
            missionView.ShortDescription = mission.ShortDescription;
            missionView.MissionId = mission.MissionId;
            missionView.StartDate = mission.StartDate;
            missionView.EndDate = mission.EndDate;
            missionView.SeatAvailable = mission.SeatAvailable;
            missionView.ThemeId = mission.ThemeId;
            missionView.Status = mission.Status;
            missionView.MissionType = mission.MissionType;
            missionView.OrganizationDetail = mission.OrganizationDetail;
            missionView.OrganizationName = mission.OrganizationName;


            return missionView;
        }
        public FavouriteMission FavouriteMission(int id, long missionId)
        {
            FavouriteMission favoriteMission = _objdb.FavouriteMissions
                .FirstOrDefault(fm => fm.UserId == id && fm.MissionId == missionId);
            return favoriteMission;
        }
        public MissionRating Rating(int id, long missionId)
        {
            MissionRating ratingExists =  _objdb.MissionRatings.FirstOrDefault(fm => fm.UserId == id && fm.MissionId == missionId);
            return ratingExists;
        }

        public MissionInvite missionInviteExists(int fromUserid,int ToUserId,long missionId)
        {
            MissionInvite missionInviteExists = _objdb.MissionInvites.FirstOrDefault(t => t.FromUserId == fromUserid && t.ToUserId == ToUserId && t.MissionId == missionId);
            return missionInviteExists;
        }

        public StoryInvite storyInviteExists(int fromUserid, int ToUserId, long storyId)
        {
            StoryInvite storyInviteExists = _objdb.StoryInvites.FirstOrDefault(t => t.FromUserId == fromUserid && t.ToUserId == ToUserId && t.StoryId == storyId);
            return storyInviteExists;
        }

        public bool ADDstoryInvite(StoryInvite InviteExixts, int fomUserId, int ToUserId, long storyId)
        {
            if (InviteExixts != null)
            {
                InviteExixts.StoryId = storyId;
                InviteExixts.UpdatedAt = DateTime.Now;

                _objdb.StoryInvites.Update(InviteExixts);
                _objdb.SaveChanges();
                return true;

            }
            else
            {
                StoryInvite Story_Invite = new StoryInvite();
                Story_Invite.StoryId = storyId;
                Story_Invite.FromUserId = fomUserId;
                Story_Invite.ToUserId = ToUserId;
                _objdb.StoryInvites.Add(Story_Invite);
                _objdb.SaveChanges();
                return true;
            }
        }


        public List<City> cities()
        {
            List<City> cities = _objdb.Cities.ToList();
            return cities;
        }

        public List<City> cities(long countryId)
        {
            List<City> cities = _objdb.Cities.Where(ct=>ct.CountryId==countryId).ToList();
            return cities;
        }
        public List<Country> countries()
        {
            List<Country> countries = _objdb.Countries.ToList();
            return countries;
        }

        public List<Skill> skills(int userid)
        {
            List<Skill> skill = new List<Skill>();
            var userskill = _objdb.UserSkills.Where(us => us.UserId == userid).ToList();
            List<Skill> skills=_objdb.Skills.Where(s=>s.Status==1 && s.DeletedAt==null).ToList();  
            foreach(var i in skills)
           {
                if (userskill.Find(x=>x.SkillId==i.SkillId)==null)
                {
                    skill.Add(i);

                }
            }
            skills = skill;
            return skills;
        }
        public List<Skill> oneuserskill(int userid)
        {
            var userskill = _objdb.UserSkills.Where(us => us.UserId == userid).ToList();
            List<Skill> skills = _objdb.Skills.Where(s => s.Status == 1 && s.DeletedAt == null).ToList();
            var oneuserskill = (from u in userskill join s in skills on u.SkillId equals s.SkillId select s).ToList();
            return oneuserskill;
        }
        public void adduser(Userviewmodel userViewModel, int userid)
        {
            User user = _objdb.Users.FirstOrDefault(u => u.UserId == userid);
            user.FirstName = userViewModel.FirstName == null ? user.FirstName : userViewModel.FirstName;
            user.LastName = userViewModel.LastName;
            user.Avatar = userViewModel.Avatar == null ? user.Avatar : userViewModel.Avatar;
            user.EmployeeId = userViewModel.EmployeeId == null ? user.EmployeeId : userViewModel.EmployeeId;
            user.Department = userViewModel.Department == null ? user.Department : userViewModel.Department;
            user.ManagerDetail = userViewModel.ManagerDetail == null ? user.ManagerDetail : userViewModel.ManagerDetail;
            user.Title = userViewModel.Title == null ? user.Title : userViewModel.Title;
            user.ProfileText = userViewModel.ProfileText == null ? user.ProfileText : userViewModel.ProfileText;
            user.WhyIVolunteer = userViewModel.WhyIVolunteer == null ? user.WhyIVolunteer : userViewModel.WhyIVolunteer;
            user.CityId = userViewModel.CityId == null ? user.CityId : userViewModel.CityId;
            user.CountryId = userViewModel.CountryId == null ? user.CountryId : userViewModel.CountryId;
            user.Availability = userViewModel.Availability == null ? user.Availability : userViewModel.Availability;
            user.LinkedInUrl = userViewModel.LinkedInUrl == null ? user.LinkedInUrl : userViewModel.LinkedInUrl;
            _objdb.Users.Update(user);
            _objdb.SaveChanges();
        }
        public void saveskill(string[] skill, int userid)
        {
            var userskill = _objdb.UserSkills.Where(us => us.UserId == userid).ToList();
            if (userskill.Count == 0)
            {
                foreach (var item in skill)
                {
                    var skillId = _objdb.Skills.Where(sk => sk.SkillName == item).Select(sk => sk.SkillId).FirstOrDefault();
                    UserSkill userSkill = new UserSkill();
                    userSkill.UserId = userid;
                    userSkill.SkillId = skillId;
                    _objdb.UserSkills.Add(userSkill);
                }
                _objdb.SaveChanges();
            }
            else
            {
                foreach (var item in userskill)
                {
                    _objdb.UserSkills.Remove(item);
                    _objdb.SaveChanges();
                }
              
                foreach (var item in skill)
                {
                    var skillId = _objdb.Skills.Where(sk => sk.SkillName == item).Select(sk => sk.SkillId).FirstOrDefault();
                    UserSkill userSkill = new UserSkill();
                    userSkill.UserId = userid;
                    userSkill.SkillId = skillId;
                    _objdb.UserSkills.Add(userSkill);
                }
                _objdb.SaveChanges();
            }
        }
        public List<Mission> missionstime(int userId)
        {
            var missionappliedbyuser = _objdb.MissionApplications.Where(ma => ma.UserId == userId && ma.ApprovalStatus == "ACCEPT").ToList();
            List<Mission> missions = _objdb.Missions.Where(m => m.MissionType == "Time" && m.DeletedAt == null).ToList();
            List<Mission> appliedbyuser = (from ma in missionappliedbyuser join ms in missions on ma.MissionId equals ms.MissionId select ms).ToList();
            return appliedbyuser;
        }
        public List<Mission> missionsgoal(int userId)
        {
            var missionappliedbyuser = _objdb.MissionApplications.Where(ma => ma.UserId == userId && ma.ApprovalStatus=="ACCEPT").ToList();
            List<Mission> missions = _objdb.Missions.Where(m => m.MissionType == "Goal" && m.DeletedAt == null).ToList();
            List<Mission> appliedbyuser = (from ma in missionappliedbyuser join ms in missions on ma.MissionId equals ms.MissionId select ms).ToList();
            return appliedbyuser;
        }
        public List<Timesheet> timesheetlist(int userId)
        {
            var usersheet = _objdb.Timesheets.Where(ts => ts.UserId == userId && ts.Action > 0).ToList();
            return usersheet;
        }
        public void timesheet(TimesheetViewModel timesheetViewModel, int userid)
        {
            if (timesheetViewModel.TimesheetId==0)
            {
                Timesheet timesheet = new Timesheet();
                timesheet.UserId = userid;
                timesheet.MissionId = timesheetViewModel.MissionId;
                timesheet.Action = int.Parse(timesheetViewModel.Action);
                timesheet.Status = "PENDING";
                timesheet.DateVolunteered = timesheetViewModel.DateVolunteered;
                timesheet.Notes = timesheetViewModel.Notes;
                _objdb.Timesheets.Add(timesheet);
                _objdb.SaveChanges();
            }
            else
            {
                var find = _objdb.Timesheets.FirstOrDefault(ts => ts.TimesheetId == timesheetViewModel.TimesheetId);
                find.MissionId = timesheetViewModel.MissionId;
                find.Action = int.Parse(timesheetViewModel.Action);
                find.DateVolunteered = timesheetViewModel.DateVolunteered;
                find.Notes = timesheetViewModel.Notes;
                _objdb.Timesheets.Update(find);
                _objdb.SaveChanges();
            }
            
        }
        public bool sheetAvail(int userid, int missionid)
        {
            var isusertimesheet = _objdb.Timesheets.FirstOrDefault(ts => ts.MissionId == missionid && ts.UserId == userid);
            if (isusertimesheet != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void contactadd(string name, string mail, string subject, string message, int userid)
        {
            ContactU contactU = new ContactU();
            contactU.UserId = userid;
            contactU.UserName = name;
            contactU.Email = mail;
            contactU.Subject = subject;
            contactU.Message = message;
            _objdb.ContactUs.Add(contactU);
            _objdb.SaveChanges();
        }
        public bool savePassword(string old, string newp, string confp, int userid)
        {
            var user = _objdb.Users.FirstOrDefault(u => u.UserId == userid);


             if (user.Password == newp)
            {
                return false;
            }
            else if (user.Password == old)
            {
                user.Password = newp;
                _objdb.Users.Update(user);
                _objdb.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Update_favourite(FavouriteMission favoriteMission,long missionId,int u_id)
        {
            if (favoriteMission != null)
            {
                if (favoriteMission.DeletedAt != null) {
                   //If fav mission is deleted than update
                    favoriteMission.UpdatedAt = DateTime.Now;
                    favoriteMission.DeletedAt = null;
                    _objdb.FavouriteMissions.Update(favoriteMission);
                    _objdb.SaveChanges();
                    return true;
                   
                }
                else
                {
                    // Remove the favorite mission from the database if it already exists
                    favoriteMission.UpdatedAt = DateTime.Now;
                    favoriteMission.DeletedAt = DateTime.Now;
                    _objdb.FavouriteMissions.Update(favoriteMission);
                    _objdb.SaveChanges();
                    return false;

                }
            }
           
            else
            {
                // Add the favorite mission to the database if it does not exist
                FavouriteMission newFavoriteMission = new FavouriteMission();
                newFavoriteMission.MissionId = missionId;
                newFavoriteMission.UserId = u_id;
                _objdb.FavouriteMissions.Add(newFavoriteMission);
                 _objdb.SaveChanges();
                return true;
            }
        }
        #region fill MailviewModel
        //public MailViewModel fillMailmodel()
        //{
        //    MailViewModel mailViewModel = new MailViewModel();
        //    mailViewModel.users = _objdb.Users.ToList();
        //    mailViewModel.missions = _objdb.Missions.ToList();
        //    return (mailViewModel);
        //}

        #endregion
        public bool ADDMissionInvite(MissionInvite InviteExixts, int fomUserId, int ToUserId, long missionId)
        {
            if (InviteExixts != null)
            {
                InviteExixts.MissionId = missionId;
                InviteExixts.UpdatedAt = DateTime.Now;

                _objdb.MissionInvites.Update(InviteExixts);
                _objdb.SaveChanges();
                return true;

            }
            else
            {
                MissionInvite Mission_Invite = new MissionInvite();
                Mission_Invite.MissionId=missionId;
                Mission_Invite.FromUserId = fomUserId;
                Mission_Invite.ToUserId = ToUserId;
                _objdb.MissionInvites.Add(Mission_Invite);
                _objdb.SaveChanges();
                return true;
            }
        }

        public List<Timesheet> timesheetlistTime(int userId)
        {
            var list = _objdb.Timesheets.Where(ts => ts.UserId == userId && ts.Action == 0).ToList();
            return list;
        }
        public void sheetime(TimesheetViewModel timesheetViewModel, int userid)
        {
            if (timesheetViewModel.TimesheetId == 0)
            {
                Timesheet timesheet = new Timesheet();
                timesheet.UserId = userid;
                timesheet.MissionId = timesheetViewModel.MissionId;
                timesheet.Time = timesheetViewModel.Timehour + ":" + timesheetViewModel.Timeminute;
                timesheet.DateVolunteered = timesheetViewModel.DateVolunteered;
                timesheet.Notes = timesheetViewModel.Notes;
                timesheet.Action = 0;
                timesheet.Status = "PENDING";
                _objdb.Timesheets.Add(timesheet);
                _objdb.SaveChanges();
            }
            else
            {
                var find = _objdb.Timesheets.FirstOrDefault(ts => ts.TimesheetId == timesheetViewModel.TimesheetId);
                find.MissionId = timesheetViewModel.MissionId;
                find.Action = 0;
                find.DateVolunteered = timesheetViewModel.DateVolunteered;
                find.Notes = timesheetViewModel.Notes;
                find.Time = timesheetViewModel.Timehour + ":" + timesheetViewModel.Timeminute;
                _objdb.Timesheets.Update(find);
                _objdb.SaveChanges();
            }
        }
        #region LoadBanner

        public DisplayBannerModel LoadBannerGet()
        {
            var banners = _objdb.Banners.Where(b => b.DeletedAt == null).ToList();
            var sortedBanners = banners.OrderBy(b => b.SortOrder).ToList();
            DisplayBannerModel bannerModel = new()
            {
                Banners = sortedBanners,
            };
            return bannerModel;
        }
        #endregion
        public bool Update_Rating(MissionRating ratingExists,string rating,int u_id,long missionId)
        {
            if (ratingExists != null)
            {
                ratingExists.Rating = rating;
                //ratingExists.UserId = id;
                //ratingExists.MissionId = missionId;
                ratingExists.UpdatedAt = DateTime.Now;
                _objdb.MissionRatings.Update(ratingExists);
                _objdb.SaveChanges();
                return true;

            }
            else
            {
                var newRating = new MissionRating();
                newRating.Rating = rating;
                newRating.UserId = u_id;
                newRating.MissionId = missionId;
                
                
                _objdb.MissionRatings.Add(newRating);
                _objdb.SaveChanges();
                return true;
            }
        }
        public bool Add_Comment(string commentText,int missionId, int u_id)
        {
            var newComment = new Comment();
            newComment.MissionId = missionId;
            newComment.UserId = u_id;
            newComment.CommentText = commentText;
            _objdb.Comments.Add(newComment);
            _objdb.SaveChanges();
            return true;
        }
        public MissionRating ADD_Rating(MissionRating ratingExists, string rating, int u_id, long missionId)
        {
            if (ratingExists != null)
            {
                ratingExists.Rating = rating;
                //ratingExists.UserId = id;
                //ratingExists.MissionId = missionId;
                _objdb.MissionRatings.Update(ratingExists);
                _objdb.SaveChangesAsync();
                return ratingExists;

            }
            else
            {
                var newRating = new MissionRating();
                newRating.Rating = rating;
                newRating.UserId = u_id;
                newRating.MissionId = missionId;


                _objdb.MissionRatings.AddAsync(newRating);
                _objdb.SaveChanges();
                return newRating;
            }
        }
        public List<CommentViewModel> getcomment(int id)
        {
            List<Comment> comments = _objdb.Comments.Where(c=>c.MissionId==id).ToList();
            List<CommentViewModel> commentsViewModel = new List<CommentViewModel>();
            foreach (var comment in comments)
            {
               CommentViewModel commentViewModel = new CommentViewModel();
                commentViewModel.CommentId=comment.MissionId;
                commentViewModel.CommentText=comment.CommentText;
                commentViewModel.ApprovalStatus = comment.ApprovalStatus;
                commentViewModel.CreatedAt = comment.CreatedAt;
                commentViewModel.DeletedAt = comment.DeletedAt;
                commentViewModel.UpadetdAt = comment.UpadetdAt;
                var user = _objdb.Users.FirstOrDefault(t => t.UserId == comment.UserId && comment.MissionId == id);
                commentViewModel.UserName = user.FirstName;
                commentViewModel.Avatar=user.Avatar;
                commentsViewModel.Add(commentViewModel);
            }
            return commentsViewModel;
        }
        public List<MissionViewModel> getrelatedmissions(int id,int u_id)
        {

           Mission missions = (Mission)_objdb.Missions.Where(m => m.MissionId == id).FirstOrDefault();
            List<Mission> Relatedmission = new List<Mission>();
            //List<Mission> Relatedmission = _objdb.Missions.Where(m=> m.CityId == missions.CityId && m.MissionId!=missions.MissionId && (m.seatsLeft == 0 || progressBar == 100 || i.Deadline < DateTime.Now)).Take(3).ToList();
            
                List<Mission> listtemp = new List<Mission>();
                listtemp = _objdb.Missions.Where(m => m.CityId == missions.CityId && m.MissionId != missions.MissionId).Take(3).ToList();
                foreach (var i in listtemp)
                {
                    var seatsLeft = (int.Parse(i.SeatAvailable) - (_objdb.MissionApplications.Where(x => x.MissionId == i.MissionId && x.ApprovalStatus == "1").Count()));
                    var progressBar = _objdb.GoalMissions.FirstOrDefault(x => x.MissionId == i.MissionId)?.GoalValue;
                    bool t = Relatedmission.Any(x => x.MissionId == i.MissionId);
                    if (t == false && (seatsLeft > 0 || progressBar < 100 || i.Deadline > DateTime.Now))
                    {
                        Relatedmission.Add(i);
                    }

                }

            
            if (Relatedmission.Count<3)
            {
                List<Mission> listtemp1=new List<Mission>();
                listtemp1 = _objdb.Missions.Where(m => m.CountryId == missions.CountryId && m.MissionId != missions.MissionId).Take(3-(Relatedmission.Count)).ToList();
                foreach(var i in listtemp1)
                {
                    var seatsLeft = (int.Parse(i.SeatAvailable) - (_objdb.MissionApplications.Where(x => x.MissionId == i.MissionId && x.ApprovalStatus == "1").Count()));
                    var progressBar = _objdb.GoalMissions.FirstOrDefault(x => x.MissionId == i.MissionId)?.GoalValue;
                    bool t = Relatedmission.Any(x => x.MissionId == i.MissionId);
                    if (t == false && (seatsLeft > 0 || progressBar < 100 || i.Deadline > DateTime.Now))
                    {
                        Relatedmission.Add(i);
                    }
                   
                }
                if (Relatedmission.Count<3)
                {
                    List<Mission> listtemp2 = new List<Mission>();
                    listtemp2 = _objdb.Missions.Where(m => m.ThemeId == missions.ThemeId && m.MissionId != missions.MissionId).Take(3 - (Relatedmission.Count)).ToList();
                    foreach (var i in listtemp2)
                    {
                        var seatsLeft = (int.Parse(i.SeatAvailable) - (_objdb.MissionApplications.Where(x => x.MissionId == i.MissionId && x.ApprovalStatus == "1").Count()));
                        var progressBar = _objdb.GoalMissions.FirstOrDefault(x => x.MissionId == i.MissionId)?.GoalValue;
                        bool t = Relatedmission.Any(x => x.MissionId == i.MissionId);
                        if (t == false && (seatsLeft > 0 || progressBar < 100 || i.Deadline > DateTime.Now))
                        {
                            Relatedmission.Add(i);
                        }
                        
                    }
                }
               
            }
            

            List<MissionViewModel> missionViews = new List<MissionViewModel>();
            foreach (var mission in Relatedmission)
            {
                MissionViewModel missionView = new MissionViewModel();
                missionView.Availability = mission.Availability;
                missionView.Title = mission.Title;
                var city = _objdb.Cities.SingleOrDefault(c => c.CityId == mission.CityId);
                if (city != null)
                {
                    missionView.City = city.Name;
                }

                var country = _objdb.Countries.SingleOrDefault(c => c.CountryId == mission.CountryId);
                if (country != null)
                {
                    missionView.Country = country.Name;
                }
                var Theme = _objdb.MissionThemes.SingleOrDefault(t => t.MissionThemeId == mission.ThemeId);
                if (Theme != null)
                {
                    missionView.Theme = Theme.Title;
                }

                var image = _objdb.MissionMedia.FirstOrDefault(t => t.MissionId == mission.MissionId);
                if (image != null)
                {
                    missionView.MediaPath = image.MediaPath;
                }
                var images = _objdb.MissionMedia.Where(t => t.MissionId == mission.MissionId).ToList();
                if (images != null)
                {
                    var mediaPaths = new List<string>();
                    foreach (var image1 in images)
                    {
                        mediaPaths.Add(image1.MediaPath);
                    }
                    missionView.MediaPaths = mediaPaths;
                }


                var goalvalue = _objdb.GoalMissions.SingleOrDefault(t => t.MissionId == mission.MissionId);
                if (goalvalue != null)
                {
                    missionView.GoalValue = goalvalue.GoalValue;
                }
                var goaltext = _objdb.GoalMissions.SingleOrDefault(t => t.MissionId == mission.MissionId);
                if (goalvalue != null)
                {
                    missionView.GoalObjectiveText = goalvalue.GoalObjectiveText;
                }

                
                var ratings = _objdb.MissionRatings.Where(t => t.MissionId == mission.MissionId).ToList();
                var count=ratings.Count;
            missionView.ratingCount= count;
                float rating = 0;
                float sum = 0;
                foreach (var entry in ratings)
                {
                    sum = sum + int.Parse(entry.Rating);

                }
                rating = sum / ratings.Count;
                if (rating != null)
                {
                    missionView.avgRating = rating;
                }
                var favourite = _objdb.FavouriteMissions.Where(x => x.MissionId == mission.MissionId && x.UserId == u_id && x.DeletedAt == null).ToList();
                if (favourite.Count > 0)
                {
                    missionView.isFavrouite = true;
                }
                else
                {
                    missionView.isFavrouite = false;
                }
                var Applided = _objdb.MissionApplications.Where(e => e.MissionId == mission.MissionId && e.UserId == u_id).ToList();
                var Pending = _objdb.MissionApplications.Where(e => e.MissionId == mission.MissionId && e.UserId == u_id && e.ApprovalStatus == "PENDING").ToList();
                if (Applided.Count > 0)
                {
                    missionView.isApplied = true;
                }
                else
                {
                    missionView.isApplied = false;
                }
                if (Pending.Count > 0)
                {
                    missionView.isPending = true;
                }
                else
                {
                    missionView.isPending = false;
                }
                missionView.deadline = mission.Deadline;
                missionView.Description = mission.Description;
                missionView.CityId = mission.CityId;
                missionView.CountryId = mission.CountryId;
                missionView.CreatedAt = mission.CreatedAt;
                missionView.UpdatedAt = mission.UpdatedAt;
                missionView.DeletedAt = mission.DeletedAt;
                missionView.ShortDescription = mission.ShortDescription;
                missionView.MissionId = mission.MissionId;
                missionView.StartDate = mission.StartDate;
                missionView.EndDate = mission.EndDate;
                missionView.SeatAvailable = mission.SeatAvailable;
                missionView.ThemeId = mission.ThemeId;
                missionView.Status = mission.Status;
                missionView.MissionType = mission.MissionType;
                missionView.OrganizationDetail = mission.OrganizationDetail;
                missionView.OrganizationName = mission.OrganizationName;
                missionViews.Add(missionView);
            }
            return missionViews;

        }
        public User getuserEmail(string mail)
        {
            return _objdb.Users.FirstOrDefault(t=>t.Email == mail); 
        }

        public List<MissionDocument> getmissionDocument(int missionid)
        {
            return _objdb.MissionDocuments.Where(t=>t.MissionId==missionid).ToList();
        }
        public Timesheet findtimerecord(int timesheetid)
        {
            var find = _objdb.Timesheets.FirstOrDefault(ts => ts.TimesheetId == timesheetid);
            return find;
        }
        public ShareStoryViewModel getsharestory(int id)
        {
            var tempMission = _objdb.MissionApplications.Where(x => x.UserId == id && x.ApprovalStatus == "ACCEPT").Select(x => x.MissionId).ToList();
            List<Mission>list=  new List<Mission>();
            foreach (var item in tempMission)
            {
                var mission = _objdb.Missions.FirstOrDefault(x => x.MissionId == item);
                list.Add(mission);
            }
            ShareStoryViewModel shareStory=new ShareStoryViewModel();
            shareStory.missionsList = list;
            
            return shareStory;
        }
        public bool alreadystory(int MissionId, int UserId)
        {
            var story = _objdb.Stories.FirstOrDefault(st => st.MissionId == MissionId && st.UserId == UserId);
            if (story == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public long story(string[] Image, int MissionId, string Title, DateTime Date, string Description, int UserId,string[] videoUrl)
        {
            Story story = new Story();
            story.Title = Title;
            story.MissionId = MissionId;
            story.UserId = UserId;
            story.PublichedAt = Date;
            story.Description = Description;
            story.Views = 0;
            _objdb.Stories.Add(story);
            _objdb.SaveChanges();
            var matchstory = _objdb.Stories.FirstOrDefault(s => s.UserId == UserId && s.MissionId == MissionId);
            var storyid = matchstory.StoryId;

            foreach (var item in Image)
            {
                StoryMedium storymedium = new StoryMedium();
                storymedium.StoryId = matchstory.StoryId;
                storymedium.Type="image";
                storymedium.Path = item;
                _objdb.StoryMedia.Add(storymedium);
                _objdb.SaveChanges();
            }

            foreach (var item in videoUrl)
            {
                StoryMedium storymedium = new StoryMedium();
                storymedium.StoryId = matchstory.StoryId;
                storymedium.Type = "Video";
                storymedium.Path = item;
                _objdb.StoryMedia.Add(storymedium);
                _objdb.SaveChanges();
            }
            return storyid; 
        }

        public long storySubmit(string[] Image, int MissionId, string Title, DateTime Date, string Description, int UserId, string[] videoUrl)
        {
            var story1 = _objdb.Stories.FirstOrDefault(st => st.MissionId == MissionId && st.UserId == UserId);
            
            if (story1 != null)
            {
                long storyid = story1.StoryId;
                story1.Views = 0;
                story1.Title = Title;
                story1.MissionId = MissionId;
                story1.UserId = UserId;
                story1.PublichedAt = Date;
                story1.Description = Description;
                _objdb.Update(story1);
                _objdb.SaveChanges();
                var media = _objdb.StoryMedia.Where(sm => sm.StoryId == story1.StoryId).ToList();
                foreach (var medias in media)
                {
                    if (medias.Type=="image")
                    {
                        foreach (var item in Image)
                        {
                            medias.StoryId = story1.StoryId;
                            medias.Type = "image";
                            medias.Path = item;
                            _objdb.StoryMedia.Update(medias);
                            _objdb.SaveChanges();
                        }
                        
                    }
                    else
                    {
                        foreach (var item in videoUrl)
                        {
                            medias.StoryId = story1.StoryId;
                            medias.Type = "Video";
                            medias.Path = item;
                            _objdb.StoryMedia.Update(medias);
                            _objdb.SaveChanges();
                        }
                    }
                }
                
                return storyid;
            }
            else
            {
                Story story = new Story();
                story.Title = Title;
                story.MissionId = MissionId;
                story.UserId = UserId;
                story.PublichedAt = Date;
                story.Description = Description;
                story.Views = 0;
                _objdb.Stories.Add(story);
                _objdb.SaveChanges();
                var matchstory = _objdb.Stories.FirstOrDefault(s => s.UserId == UserId && s.MissionId == MissionId);
                var storyid = matchstory.StoryId;

                foreach (var item in Image)
                {
                    StoryMedium storymedium = new StoryMedium();
                    storymedium.StoryId = matchstory.StoryId;
                    storymedium.Type = "image";
                    storymedium.Path = item;
                    _objdb.StoryMedia.Add(storymedium);
                    _objdb.SaveChanges();
                }

                foreach (var item in videoUrl)
                {
                    StoryMedium storymedium = new StoryMedium();
                    storymedium.StoryId = matchstory.StoryId;
                    storymedium.Type = "Video";
                    storymedium.Path = item;
                    _objdb.StoryMedia.Add(storymedium);
                    _objdb.SaveChanges();
                }
                return storyid;
            }
            
        }
        //public Story story(string[] Image, int MissionId, string Title, DateTime Date, string Description, int UserId, string Value)
        //{
        //    var story = _objdb.Stories.FirstOrDefault(st => st.MissionId == MissionId && st.UserId == UserId);

        //    if (story == null && Value == "save")
        //    {
        //        Story storys = new Story();
        //        storys.Title = Title;
        //        storys.MissionId = MissionId;
        //        storys.UserId = UserId;
        //        storys.PublishedAt = Date;
        //        storys.StoryDescription = Description;
        //        _objdb.Stories.Add(storys);
        //        _objdb.SaveChanges();
        //        var matchstory = _objdb.Stories.FirstOrDefault(s => s.UserId == UserId && s.MissionId == MissionId);

        //        foreach (var item in Image)
        //        {
        //            StoryMedium storymedium = new StoryMedium();
        //            storymedium.StoryId = matchstory.StoryId;
        //            storymedium.Path = item;
        //            _objdb.StoryMedia.Add(storymedium);
        //        }
        //        _objdb.SaveChanges();
        //        return matchstory;
        //    }
        //    else if (story == null && Value == "submit")
        //    {
        //        Story storys = new Story();
        //        storys.Title = Title;
        //        storys.MissionId = MissionId;
        //        storys.UserId = UserId;
        //        storys.PublishedAt = Date;
        //        storys.Status = "pending";
        //        storys.StoryDescription = Description;
        //        _objdb.Stories.Add(storys);
        //        _objdb.SaveChanges();
        //        var matchstory = _objdb.Stories.FirstOrDefault(s => s.UserId == UserId && s.MissionId == MissionId);

        //        foreach (var item in Image)
        //        {
        //            StoryMedium storymedium = new StoryMedium();
        //            storymedium.StoryId = matchstory.StoryId;
        //            storymedium.Path = item;
        //            _objdb.StoryMedia.Add(storymedium);
        //        }
        //        _objdb.SaveChanges();
        //        return matchstory;
        //    }

        //    else if (story != null && Value == "save")
        //    {
        //        story.MissionId = MissionId;
        //        story.UserId = UserId;
        //        story.Title = Title;
        //        story.PublishedAt = Date;
        //        story.StoryDescription = Description;
        //        _objdb.Stories.Update(story);
        //        var storymediums = _objdb.StoryMedia.FirstOrDefault(sm => sm.StoryId == story.StoryId);
        //        foreach (var item in Image)
        //        {

        //            storymediums.StoryId = story.StoryId;
        //            storymediums.Path = item;
        //            _objdb.StoryMedia.Update(storymediums);
        //        }
        //        _objdb.SaveChanges();
        //        return story;
        //    }
        //    else
        //    {
        //        story.MissionId = MissionId;
        //        story.UserId = UserId;
        //        story.Title = Title;
        //        story.PublishedAt = Date;
        //        story.Status = "pending";
        //        story.StoryDescription = Description;
        //        _objdb.Stories.Update(story);
        //        var storymediums = _objdb.StoryMedia.FirstOrDefault(sm => sm.StoryId == story.StoryId);
        //        foreach (var item in Image)
        //        {

        //            storymediums.StoryId = story.StoryId;
        //            storymediums.Path = item;
        //            _objdb.StoryMedia.Update(storymediums);
        //        }
        //        _objdb.SaveChanges();
        //        return story;
        //    }
        //}
        public Story getstory(string[] Image, int MissionId, string Title, DateTime Date, string Description, int UserId, string[] videoUrl, string value)
        {
            var story1 = _objdb.Stories.FirstOrDefault(st => st.MissionId == MissionId && st.UserId == UserId);
            if (story1 == null && value == "save")
            {
                Story storys = new Story();
                storys.Title = Title;
                storys.MissionId = MissionId;
                storys.UserId = UserId;
                storys.PublichedAt = Date;
                storys.Views = 0;
                storys.Description = Description;
                _objdb.Stories.Add(storys);
                _objdb.SaveChanges();
                var matchstory = _objdb.Stories.FirstOrDefault(s => s.UserId == UserId && s.MissionId == MissionId);

                foreach (var item in Image)
                {
                    StoryMedium storymedium = new StoryMedium();
                    storymedium.StoryId = matchstory.StoryId;
                    storymedium.Type = "image";
                    storymedium.Path = item;
                    _objdb.StoryMedia.Add(storymedium);
                    _objdb.SaveChanges();
                }

                foreach (var item in videoUrl)
                {
                    StoryMedium storymedium = new StoryMedium();
                    storymedium.StoryId = matchstory.StoryId;
                    storymedium.Type = "Video";
                    storymedium.Path = item;
                    _objdb.StoryMedia.Add(storymedium);
                    _objdb.SaveChanges();
                }
                return matchstory;
            }
            else if (story1 == null && value == "submit")
            {
                Story storys = new Story();
                storys.Title = Title;
                storys.MissionId = MissionId;
                storys.UserId = UserId;
                storys.PublichedAt = Date;
                storys.Views = 0;
                storys.Status = "PENDING";
                storys.Description = Description;
                _objdb.Stories.Add(storys);
                _objdb.SaveChanges();
                var matchstory = _objdb.Stories.FirstOrDefault(s => s.UserId == UserId && s.MissionId == MissionId);

                foreach (var item in Image)
                {
                    StoryMedium storymedium = new StoryMedium();
                    storymedium.StoryId = matchstory.StoryId;
                    storymedium.Type = "image";
                    storymedium.Path = item;
                    _objdb.StoryMedia.Add(storymedium);
                    _objdb.SaveChanges();
                }

                foreach (var item in videoUrl)
                {
                    StoryMedium storymedium = new StoryMedium();
                    storymedium.StoryId = matchstory.StoryId;
                    storymedium.Type = "Video";
                    storymedium.Path = item;
                    _objdb.StoryMedia.Add(storymedium);
                    _objdb.SaveChanges();
                }
                return matchstory;
            }
            else if (story1 != null && value == "save")
            {
                story1.MissionId = MissionId;
                story1.UserId = UserId;
                story1.MissionId = MissionId;
                story1.UserId = UserId;
                story1.PublichedAt = Date;
                story1.Description = Description;
                _objdb.Update(story1);
                _objdb.SaveChanges();
                var media = _objdb.StoryMedia.Where(sm => sm.StoryId == story1.StoryId).ToList();
                
                foreach (var item in media)
                {
                    _objdb.Remove(item);
                    _objdb.SaveChanges();

                }
                foreach (var item in Image)
                {
                    StoryMedium storymedium = new StoryMedium();
                    storymedium.StoryId = story1.StoryId;
                    storymedium.Type = "image";
                    storymedium.Path = item;
                    _objdb.StoryMedia.Add(storymedium);
                    _objdb.SaveChanges();
                }
                foreach (var item in videoUrl)
                {
                    StoryMedium storymedium = new StoryMedium();
                    storymedium.StoryId = story1.StoryId;
                    storymedium.Type = "Video";
                    storymedium.Path = item;
                    _objdb.StoryMedia.Add(storymedium);
                    _objdb.SaveChanges();
                }


                return story1;
            }
            else
            {
                story1.MissionId = MissionId;
                story1.UserId = UserId;
                story1.MissionId = MissionId;
                story1.UserId = UserId;
                story1.Status = "PENDING";
                story1.PublichedAt = Date;
                story1.Description = Description;
                _objdb.Update(story1);
                _objdb.SaveChanges();
                var media = _objdb.StoryMedia.Where(sm => sm.StoryId == story1.StoryId).ToList();

                foreach (var item in media)
                {
                    _objdb.Remove(item);
                    _objdb.SaveChanges();

                }
                foreach (var item in Image)
                {
                    StoryMedium storymedium = new StoryMedium();
                    storymedium.StoryId = story1.StoryId;
                    storymedium.Type = "image";
                    storymedium.Path = item;
                    _objdb.StoryMedia.Add(storymedium);
                    _objdb.SaveChanges();
                }
                foreach (var item in videoUrl)
                {
                    StoryMedium storymedium = new StoryMedium();
                    storymedium.StoryId = story1.StoryId;
                    storymedium.Type = "Video";
                    storymedium.Path = item;
                    _objdb.StoryMedia.Add(storymedium);
                    _objdb.SaveChanges();
                }


                return story1;
            }


           
        }
    }
}
