using CI_Platform.Entity.CIDbContext;
using CI_Platform.Entity.ViewModels;
using CI_Platform.Entity.DataModels;
using CI_Platform.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repository
{
    public class LandingRepository : ILandingRepository
    {
        private readonly CiPlatformContext _CiPlatformContext;
        public LandingRepository(CiPlatformContext CiPlatformContext)
        {
            _CiPlatformContext = CiPlatformContext;
        }

        #region Landing Page
        public MissionLandingModel LandingPageGet()
        {
            MissionLandingModel model = new MissionLandingModel()
            {
                City = _CiPlatformContext.Cities.ToList(),
                Country = _CiPlatformContext.Countries.ToList(),
                MissionThemes = _CiPlatformContext.MissionThemes.ToList()

            };
            return model;
        }

        public MissionLandingModel LandingPagePost(string[]? country, string[]? city, string[]? theme, string? searchTerm, string? sortValue, int pg)
        {
            List<Mission> miss = _CiPlatformContext.Missions.ToList();


            MissionLandingModel model = new MissionLandingModel
            {
                Missions = _CiPlatformContext.Missions.ToList(),
                Country = _CiPlatformContext.Countries.ToList(),
                City = _CiPlatformContext.Cities.ToList(),
                MissionThemes = _CiPlatformContext.MissionThemes.ToList(),
                MissionSkills = _CiPlatformContext.MissionSkills.ToList(),
                GoalMission = _CiPlatformContext.GoalMissions.ToList(),
                FavMissionData = _CiPlatformContext.FavoriteMissions.ToList(),
                MissionApplications = _CiPlatformContext.MissionApplications.ToList(),
                MissionRatings = _CiPlatformContext.MissionRatings.ToList(),
                Users = _CiPlatformContext.Users.ToList(),


            };


            if (country.Count() > 0 || city.Count() > 0 || theme.Count() > 0)
            {
                miss = GetFilteredMission(miss, country, city, theme);
            }



            if (searchTerm != null)
            {
                miss = miss.Where(m => m.Title.ToLower().Contains(searchTerm)).ToList();

            }



            miss = GetSortedMissions(miss, sortValue);

            model.Missions = miss;

            return model;
        }

        #endregion

        #region GetSortedMissions Method
        public List<Mission> GetSortedMissions(List<Mission> miss, string sortValue)
        {
            switch (sortValue)
            {
                case "Newest":
                    return miss.OrderBy(m => m.StartDate).ToList();
                case "Oldest":
                    return miss.OrderByDescending(m => m.StartDate).ToList();
                case "lowest":
                    return miss.OrderBy(m => m.Availability).ToList();
                case "highest":
                    return miss.OrderByDescending(m => m.Availability).ToList();
                default:
                    return miss.ToList();

            }
        }

        #endregion

        #region GetFilteredMission Method
        public List<Mission> GetFilteredMission(List<Mission> miss, string[] country, string[] city, string[] theme)
        {
            if (country.Length > 0)
            {
                miss = miss.Where(m => country.Contains(m.Country.Name)).ToList();
            }

            if (city.Length > 0)
            {
                miss = miss.Where(m => city.Contains(m.City.Name)).ToList();
            }

            if (theme.Length > 0)
            {
                miss = miss.Where(m => theme.Contains(m.Theme.Title)).ToList();
            }

            return miss;
        }

        #endregion

        #region Volunteering Page

        public VolunteeringMissionModel MissionVolunteering(int MissionId, long UserId)
        {
            List<Mission> missionlist = _CiPlatformContext.Missions.ToList();


            List<User> AllUsers = new List<User>();
            AllUsers = _CiPlatformContext.Users.ToList();


            //List<Comment> comments = _CimainContext.Comments.ToList();
            var comments = from U in _CiPlatformContext.Users join CM in _CiPlatformContext.Comments on U.UserId equals CM.UserId where CM.MissionId == MissionId select new { U.FirstName, CM.CommentText, CM.CreatedAt };


            //var userid = IHttpContextAccessor.Session.GetString("UserId");


            var mission = _CiPlatformContext.Missions.FirstOrDefault(m => m.MissionId == MissionId);
            var favmission = _CiPlatformContext.FavoriteMissions.FirstOrDefault(FM => FM.MissionId == MissionId && FM.UserId == UserId);
            missionlist = missionlist.Where(t => t.ThemeId == mission.ThemeId && t.MissionId != mission.MissionId).Take(3).ToList();
            var theme = _CiPlatformContext.MissionThemes.FirstOrDefault(t => t.MissionThemeId == mission.ThemeId);
            var goaltxt = _CiPlatformContext.GoalMissions.FirstOrDefault(g => g.MissionId == mission.MissionId);
            var city = _CiPlatformContext.Cities.FirstOrDefault(s => s.CityId == mission.CityId);
            var ratings = _CiPlatformContext.MissionRatings.FirstOrDefault(MR => MR.MissionId == MissionId && MR.UserId == UserId);
            //var recvoldet = from U in CimainContext.Users join MA in CimainContext.MissionApplications on U.UserId equals MA.UserId where MA.MissionId == mission.MissionId select U;
            GoalMission goalMission = _CiPlatformContext.GoalMissions.Where(gm => gm.MissionId == mission.MissionId).FirstOrDefault();
            string[] startDateNtime = mission.StartDate.ToString().Split(' ');
            string[] endDateNtime = mission.EndDate.ToString().Split(' ');

            //List<Mission> missionlist1 = _CimainContext.Missions.ToList();
            //missionlist1 = missionlist.Where(t => t.ThemeId == mission.ThemeId && t.MissionId != mission.MissionId).Take(3).ToList();
            //ViewBag.related = missionlist1;


            //recent volunteers
            var recvoldetails = from U in _CiPlatformContext.Users join MA in _CiPlatformContext.MissionApplications on U.UserId equals MA.UserId where MA.MissionId == mission.MissionId select U;



            //Related Mission List
            List<VolunteeringMissionModel> relatedMissionList = new List<VolunteeringMissionModel>();
            var relatedmission = _CiPlatformContext.Missions.Where(m => m.ThemeId == mission.ThemeId && m.MissionId != mission.MissionId).ToList();
            foreach (var item in relatedmission.Take(3))
            {
                var relcity = _CiPlatformContext.Cities.FirstOrDefault(m => m.CityId == item.CityId);
                var reltheme = _CiPlatformContext.MissionThemes.FirstOrDefault(m => m.MissionThemeId == item.ThemeId);
                var relgoalobj = _CiPlatformContext.GoalMissions.FirstOrDefault(m => m.MissionId == item.MissionId);
                var Startdate = item.StartDate;
                var Enddate = item.EndDate;

                relatedMissionList.Add(new VolunteeringMissionModel
                {
                    MissionId = item.MissionId,
                    City = relcity.Name,
                    Theme = reltheme.Title,
                    SingleTitle = item.Title,
                    Description = item.ShortDescription,
                    StartDate = Startdate,
                    EndDate = Enddate,

                    Organization = item.OrganizationName,

                    GoalText = relgoalobj != null ? relgoalobj.GoalObjectiveText : "null",
                    missionType = item.MissionType,


                });
            }

            //ViewBag.relatedmission = relatedMissionList.Take(3);


            //Single Mission Page - Volunteering Mission Page

            var ratingTotal = _CiPlatformContext.MissionRatings.Where(MR => MR.MissionId == mission.MissionId).ToList();
            var prevRating = _CiPlatformContext.MissionRatings.FirstOrDefault(pr => pr.MissionId == mission.MissionId && pr.UserId == UserId);

            float rat = 0;
            float sum = 0;
            foreach (var r in ratingTotal)
            {
                sum = (int)(sum + (r.Rating));
            }
            if (ratingTotal.Count() == 0)
            {
                rat = 0;
            }
            else
            {
                rat = sum / ratingTotal.Count();

            }
            if (prevRating != null)
            {
                //ViewBag.avgrating = rat;
            }
            else
            {
                //ViewBag.avgrating = 0;
            }
            var TotalRatedByUser = ratingTotal.Count();
            var userapplied = _CiPlatformContext.MissionApplications.FirstOrDefault(ma => ma.MissionId == MissionId && ma.UserId == UserId);
            VolunteeringMissionModel volunteeringMission = new();
            volunteeringMission = new()
            {
                SingleTitle = mission.Title,
                Description = mission.Description,
                OrganizationDetails = mission.OrganizationDetail,
                GoalText = goalMission != null ? goalMission.GoalObjectiveText : "null",
                StartDate = (DateTime)mission.StartDate,
                EndDate = (DateTime)mission.EndDate,
                StartDateEndDate = "From " + startDateNtime[0] + " until " + endDateNtime[0],
                missionType = mission.MissionType,
                MissionId = mission.MissionId,
                City = city.Name,
                Theme = theme.Title,
                Organization = mission.OrganizationName,
                Rating = ratings != null ? ratings.Rating : 0,
                isFavrouite = favmission == null ? null : favmission.MissionId,
                UserId = favmission == null ? null : favmission.UserId.ToString(),
                AverageRating = rat,
                TotalRatedByUsers = TotalRatedByUser,
                Comments = _CiPlatformContext.Comments.Where(CM => CM.MissionId == MissionId).ToList() != null ? _CiPlatformContext.Comments.Where(CM => CM.MissionId == MissionId).ToList() : null,
                AllUsers = _CiPlatformContext.Users.ToList(),
                RecentVolunteers = _CiPlatformContext.MissionApplications.ToList(),
                userApplied = userapplied == null ? true : false


            };



            return volunteeringMission;
        }
        #endregion

        #region AddRating

        public MissionRating AddRating(int rating, long Id, long missionId)
        {
            MissionRating ratingExists = _CiPlatformContext.MissionRatings.FirstOrDefault(fm => fm.UserId == Id && fm.MissionId == missionId);
            if (ratingExists != null)
            {
                ratingExists.Rating = rating;
                _CiPlatformContext.Update(ratingExists);
                _CiPlatformContext.SaveChanges();


                return ratingExists;
            }
            else
            {
                var ratingele = new MissionRating();
                ratingele.Rating = rating;
                ratingele.UserId = Id;
                ratingele.MissionId = missionId;
                _CiPlatformContext.Add(ratingele);
                _CiPlatformContext.SaveChanges();
                return ratingele;
            }

        }

        #endregion

        #region AddComment

        public VolunteeringMissionModel PostComment(long MissionId, long UserId, string CommentText)
        {

            var newComment = new Comment();
            newComment.MissionId = MissionId;
            newComment.UserId = UserId;
            newComment.CommentText = CommentText;
            _CiPlatformContext.Add(newComment);
            var saved = _CiPlatformContext.SaveChanges();

            if (saved != null)
            {
                VolunteeringMissionModel model = new()
                {
                    Comments = _CiPlatformContext.Comments.Where(CM => CM.MissionId == MissionId).ToList(),
                    AllUsers = _CiPlatformContext.Users.ToList(),
                };
                return model;
            }
            else
            {
                VolunteeringMissionModel model = new()
                {
                    Comments = null,
                    AllUsers = null,
                };
                return model;
            }

        }

        #endregion

        #region AddToFav Method
        public bool AddToFav(long MissionId, long UserId)
        {
            FavoriteMission favoriteMission = _CiPlatformContext.FavoriteMissions.FirstOrDefault(FM => FM.MissionId == MissionId && FM.UserId == UserId);

            if (favoriteMission != null)
            {
                FavoriteMission favoriteMissiondel = _CiPlatformContext.FavoriteMissions.Where(FM => FM.MissionId == MissionId && FM.UserId == UserId).FirstOrDefault();
                _CiPlatformContext.Remove(favoriteMissiondel);
                _CiPlatformContext.SaveChanges();
                return false;
            }
            else
            {
                var favoriteMissionadd = new FavoriteMission();
                favoriteMissionadd.UserId = UserId;
                favoriteMissionadd.MissionId = MissionId;
                _CiPlatformContext.Add(favoriteMissionadd);
                _CiPlatformContext.SaveChanges();
                return true;
            }
        }

        #endregion

        #region RecomandUser
        public bool RecomandUser(string EmailId, int MissionId)
        {

            UriBuilder builder = new UriBuilder();
            builder.Scheme = "https";
            builder.Host = "localhost";
            builder.Port = 7283;
            builder.Path = "/LandingPage/MissionVolunteering";
            builder.Query = "MissionId=" + MissionId;
            var resetLink = builder.ToString();
            // Send email to user with reset password link
            // ...
            MailAddress fromAddress = new MailAddress("evanzandu@gmail.com", "CI Platform");
            MailAddress toAddress = new MailAddress(EmailId);
            var subject = "Someone Recomand you to join Mission";
            var body = $"Hi,<br /><br />Please click on the following link to see mission details:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("evanzandu@gmail.com", "timrrquqhqzvdpns"),
                EnableSsl = true
            };
            smtpClient.Send(message);
            return true;
        }
        #endregion

        #region Applynow
        public bool ApplyNow(long missionId, long userId)
        {
            MissionApplication missionApplications = _CiPlatformContext.MissionApplications.FirstOrDefault(FM => FM.MissionId == missionId && FM.UserId == userId);
            if (missionApplications != null)
            {
                return false;
            }
            else
            {
                MissionApplication missionApplication = new MissionApplication();
                missionApplication.MissionId = missionId;
                missionApplication.UserId = userId;
                missionApplication.AppliedAt = DateTime.Now;
                missionApplication.ApprovalStatus = "pending";
                _CiPlatformContext.Add(missionApplication);
                _CiPlatformContext.SaveChanges();
                return true;
            }
        }

        #endregion
    }
}