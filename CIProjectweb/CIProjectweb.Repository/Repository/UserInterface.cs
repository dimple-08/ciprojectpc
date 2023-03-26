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
           var userexsists= _objdb.Users.Where(a => a.Email.Equals(objuser.Email)).FirstOrDefault();
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
            var userexsists = _objdb.Users.Where(a => a.Email.Equals(objFpvm.Email)).FirstOrDefault();
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
            var userexsists = _objdb.Users.Where(a => a.Email.Equals(objreset.Email) ).FirstOrDefault();
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
                missionView.GoalValue = goalvalue.GoalValue;
            }
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
            var seats = (int.Parse(mission.SeatAvailable) - (_objdb.MissionApplications.Where(x => x.MissionId == mission.MissionId).Count()));
            missionView.SeatAvailable = seats.ToString();
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

        public ShareStoryViewModel getsharestory()
        {
           
           ShareStoryViewModel shareStory=new ShareStoryViewModel();
            shareStory.missionsList=_objdb.Missions.ToList();
            
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
        public void story(string[] Image, int MissionId, string Title, DateTime Date, string Description, int UserId,string[] videoUrl)
        {
            Story story = new Story();
            story.Title = Title;
            story.MissionId = MissionId;
            story.UserId = UserId;
            story.PublichedAt = Date;
            story.Description = Description;
            _objdb.Stories.Add(story);
            _objdb.SaveChanges();
            var matchstory = _objdb.Stories.FirstOrDefault(s => s.UserId == UserId && s.MissionId == MissionId);


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
        }
    }
}
