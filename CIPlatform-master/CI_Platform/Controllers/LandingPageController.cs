using CI_Platform.Entity.ViewModels;
using CI_Platform.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class LandingPageController : Controller
    {
        private readonly ILandingRepository _LandingRepository;

        public LandingPageController(ILandingRepository LandingRepository)
        {

            _LandingRepository = LandingRepository;

        }

        #region LandingPage Get and Post
        public IActionResult LandingPage()
        {

            var LandingPage = _LandingRepository.LandingPageGet();
            MissionLandingModel model = new MissionLandingModel()
            {
                City = LandingPage.City,
                Country = LandingPage.Country,
                MissionThemes = LandingPage.MissionThemes
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult LandingPage(string[]? country, string[]? city, string[]? theme, string? searchTerm, string? sortValue, int pg)
        {
            
            var LandingPageData = _LandingRepository.LandingPagePost(country, city, theme, searchTerm, sortValue, pg );


            ViewBag.ToalMissionCount = LandingPageData.Missions.Count();
            var userId = HttpContext.Session.GetString("UserId");

            if (userId != null)
            {

                MissionLandingModel model = new MissionLandingModel()
                {
                    City = LandingPageData.City,
                    Country = LandingPageData.Country,
                    MissionThemes = LandingPageData.MissionThemes,
                    Missions = LandingPageData.Missions,
                    FavMissionData = LandingPageData.FavMissionData.Where(Fm => Fm.UserId == long.Parse(userId)).ToList(),
                    MissionSkills = LandingPageData.MissionSkills,
                    GoalMission = LandingPageData.GoalMission,
                    MissionRatings = LandingPageData.MissionRatings,
                    Users = LandingPageData.Users,
                    MissionApplications = LandingPageData.MissionApplications.Where(ma => ma.UserId == long.Parse(userId)).ToList(),


                };

                const int pageSize = 6;
                if (pg < 1)
                {
                    pg = 1;
                }

                int missionCount = model.Missions.Count();

                var PaginationModel = new PaginationModel(missionCount, pg, pageSize);

                int missionSkip = (pg - 1) * pageSize;
                ViewBag.Pagination = PaginationModel;

                var FinalMissions = model.Missions.Skip(missionSkip).Take(PaginationModel.PageSize).ToList();



                int totalCount = model.Missions.Count();


                MissionLandingModel modelWithPagination = new MissionLandingModel()
                {
                    City = LandingPageData.City,
                    Country = LandingPageData.Country,
                    MissionThemes = LandingPageData.MissionThemes,
                    Missions = FinalMissions,
                    FavMissionData = LandingPageData.FavMissionData.Where(Fm => Fm.UserId == long.Parse(userId)).ToList(),
                    MissionSkills = LandingPageData.MissionSkills,
                    GoalMission = LandingPageData.GoalMission,
                    MissionRatings = LandingPageData.MissionRatings,
                    Users = LandingPageData.Users,
                    MissionApplications = LandingPageData.MissionApplications.Where(ma => ma.UserId == long.Parse(userId)).ToList(),

                };




                return PartialView("_MissionCards", modelWithPagination);
            }
            else
            {
                MissionLandingModel model = new MissionLandingModel()
                {
                    City = LandingPageData.City,
                    Country = LandingPageData.Country,
                    MissionThemes = LandingPageData.MissionThemes,
                    Missions = LandingPageData.Missions,
                    MissionSkills = LandingPageData.MissionSkills,
                    GoalMission = LandingPageData.GoalMission,
                    MissionRatings = LandingPageData.MissionRatings
                };
                const int pageSize = 6;
                if (pg < 1)
                {
                    pg = 1;
                }

                int missionCount = model.Missions.Count();

                var PaginationModel = new PaginationModel(missionCount, pg, pageSize);
                ViewBag.Pagination = PaginationModel;

                int missionSkip = (pg - 1) * pageSize;

                var FinalMissions = model.Missions.Skip(missionSkip).Take(PaginationModel.PageSize).ToList();




                int totalCount = model.Missions.Count();


                MissionLandingModel modelWithPagination = new MissionLandingModel()
                {
                    City = LandingPageData.City,
                    Country = LandingPageData.Country,
                    MissionThemes = LandingPageData.MissionThemes,
                    Missions = FinalMissions,
                    MissionSkills = LandingPageData.MissionSkills,
                    GoalMission = LandingPageData.GoalMission,
                    MissionRatings = LandingPageData.MissionRatings,
                    Users = LandingPageData.Users,
                };

                return PartialView("_MissionCards", modelWithPagination);
            }
        }
        #endregion

        #region Volunteering Page Get and Post
        public IActionResult MissionVolunteering(int MissionId)
        {
            
            var User = HttpContext.Session.GetString("UserId");
            if (User != null)
            {
                long UserId = long.Parse(HttpContext.Session.GetString("UserId"));

                var MissionVolunteering = _LandingRepository.MissionVolunteering(MissionId, UserId);

                VolunteeringMissionModel volunteeringMission = new()
                {
                    SingleTitle = MissionVolunteering.SingleTitle,
                    Description = MissionVolunteering.Description,
                    OrganizationDetails = MissionVolunteering.OrganizationDetails,
                    GoalText = MissionVolunteering.GoalText,
                    StartDate = MissionVolunteering.StartDate,
                    EndDate = MissionVolunteering.EndDate,
                    StartDateEndDate = MissionVolunteering.StartDateEndDate,
                    missionType = MissionVolunteering.missionType,
                    MissionId = MissionVolunteering.MissionId,
                    City = MissionVolunteering.City,
                    Theme = MissionVolunteering.Theme,
                    Organization = MissionVolunteering.Organization,
                    Rating = MissionVolunteering.Rating,
                    isFavrouite = MissionVolunteering.isFavrouite,
                    UserId = MissionVolunteering.UserId,
                    AverageRating = MissionVolunteering.AverageRating,
                    TotalRatedByUsers = MissionVolunteering.TotalRatedByUsers,
                    Comments = MissionVolunteering.Comments,
                    AllUsers = MissionVolunteering.AllUsers,
                    RecentVolunteers = MissionVolunteering.RecentVolunteers.Where(RV => RV.MissionId == MissionId).ToList(),
                    userApplied = MissionVolunteering.userApplied,
                };
                return View(volunteeringMission);
            }
            else
            {
                return RedirectToAction("Login","User");
            }
        }
        
        #endregion

        #region AddToFav
        public IActionResult AddToFav(long MissionId, long UserId)
        {
            var addToFav = _LandingRepository.AddToFav(MissionId, UserId);
            
            if (addToFav)
            {
                var missionAdd = "added";
                return Json(new { success = true, missionAdd });
            }
            else
            {

                var missionDel = "Removed";
                return Json(new { success = true, missionDel });
            }

        }

        #endregion

        #region RecomandUser
        public IActionResult RecomandUser(string EmailId, int MissionId)
        {

            var recomandUser = _LandingRepository.RecomandUser(EmailId, MissionId);
            if (recomandUser)
            {
                return Json(new { success = true });

            }
            else
            {
                return Json(new { success = false });
            }
        }

        #endregion

        #region AddComment
        public IActionResult PostComment(long MissionId, long UserId, string commenttext)
        {
            var AddComment = _LandingRepository.PostComment(MissionId, UserId, commenttext);

            if (AddComment.Comments != null)
            {
                VolunteeringMissionModel model = new()
                {

                    Comments = AddComment.Comments,
                    AllUsers = AddComment.AllUsers,
                };

                return PartialView("_Comments", model);
            }
            else
            {
                return RedirectToAction("MissionVolunteering", "LandingPage");
            }

            return View();
        }
        #endregion

        #region Addrating
        [HttpPost]
        public IActionResult Addrating(int rating, long Id, long missionId)
        {
            var AddRating = _LandingRepository.AddRating(rating, Id, missionId);
            if (AddRating != null)
            {

                var ratingUpdated = AddRating.Rating;
                //return Json(new { success = true, ratingUpdated });
                return RedirectToAction("MissionVolunteering", new { MissionId = missionId });
            }
            else
            {
                var NewRating = AddRating.Rating;
                //return Json(new { success = true, NewRating });
                return RedirectToAction("MissionVolunteering", new { MissionId = missionId });
            }
        }
        #endregion

        #region ApplyNow

        public IActionResult ApplyNow(long MissionId, long UserId)
        {
            var applynow = _LandingRepository.ApplyNow(MissionId, UserId);

            if (applynow)
            {
                var missionAdd = "applied";
                return Json(new { success = true, missionAdd });
            }
            else
            {

                var missionDel = "already applied";
                return Json(new { success = true, missionDel });
            }

        }

        #endregion
    }
}