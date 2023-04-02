
using CIProjectweb.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using CIProjectweb.Entities.DataModels;
using Microsoft.AspNetCore.Mvc;
using CIProjectweb.Models;
using System.Text.Json;
using CIProjectweb.Repository.Repository.Interface;

namespace CI_Plateform.Controllers
{
    public class MissionController : Controller
    {
        private readonly ILogin _objILogin;
        private readonly IUserInterface _objUserInterface;
       


        private readonly ILogger<MissionController> _logger;
       
        private readonly CIDbContext _db;



        public MissionController(ILogger<MissionController> logger, CIDbContext db, ILogin objLogin, IUserInterface objUserInterface)
        {
            _logger = logger;
            _objILogin = objLogin;
            _objUserInterface = objUserInterface;
            _db = db;
        }


      

        public IActionResult LandingPage(int pg = 1)
        {
            var userId = HttpContext.Session.GetString("UserId");
           
            int u_id = int.Parse(userId);
            MissionListingModel model = new MissionListingModel();

            #region Fill Country Drop-down
            List<SelectListItem> listCountry = new List<SelectListItem>();
            var tempCountry = _db.Countries.Where(m => m.DeletedAt == null).ToList();
            foreach (var country in tempCountry)
            {
                listCountry.Add(new SelectListItem() { Text = country.Name, Value = country.CountryId.ToString() });
            }
            model.countries = listCountry;
            // ViewBag.listOfCountry = new SelectList(listCountry);
            // ViewBag.listOfCountry = ;
            #endregion Fill Country Drop-down

            #region Fill City Drop-down
            List<SelectListItem> listCity = new List<SelectListItem>();
            var tempCity = _db.Cities.Where(m => m.DeletedAt == null).ToList();
            foreach (var city in tempCity)
            {
                listCity.Add(new SelectListItem() { Text = city.Name, Value = city.CityId.ToString() });
            }
            model.cities = listCity;
            #endregion Fill City Drop-down

            #region Fill MissionTheme Drop-down
            List<SelectListItem> listTheme = new List<SelectListItem>();
            var tempTheme = _db.MissionThemes.Where(m => m.DeletedAt == null).ToList();
            foreach (var theme in tempTheme)
            {
                listTheme.Add(new SelectListItem() { Text = theme.Title, Value = theme.MissionThemeId.ToString() });
            }
            model.themes = listTheme;
            #endregion Fill MissionTheme Drop-down

            #region Fill SKills Drop-down
            List<SelectListItem> listSkills = new List<SelectListItem>();
            var tempSkills = _db.Skills.Where(m => m.DeletedAt == null).ToList();
            foreach (var skill in tempSkills)
            {
                listSkills.Add(new SelectListItem() { Text = skill.SkillName, Value = skill.SkillId.ToString() });
            }
            model.skills = listSkills;
            #endregion Fill Skills Drop-down


            var cardData = new List<MissionCardModel>();

            var tempMission = _db.Missions.Where(m => m.DeletedAt == null && m.Status == true).ToList();
            model.TotalMission = tempMission.Count;
            foreach (var mission in tempMission)
            {

                cardData.Add(CreateCard(mission));
            }
            model.missionsCard = cardData;

            const int pageSize = 9;
            int recsCount = tempMission.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            User user = _objILogin.GetUsers(u_id);
            
            ViewBag.user = user;
            var data = cardData.Skip(recSkip).Take(pager.PageSize).ToList();
            model.missionsCard = data;
            this.ViewBag.Pager = pager;
            this.ViewBag.count = recsCount;
            return View(model);
        }


        [HttpPost]
        public IActionResult LandingPage(int? id, int pg = 1)
        {
          
            MissionListingModel model = new MissionListingModel();
            List<SelectListItem> listCity = new List<SelectListItem>();
            if (id != null)
            {
                #region Fill City Drop-down

                var tempCity = _db.Cities.Where(m => m.DeletedAt == null && m.CountryId == id).ToList();
                foreach (var city in tempCity)
                {
                    listCity.Add(new SelectListItem() { Text = city.Name, Value = city.CityId.ToString() });
                }
                #endregion Fill City Drop-down
            }
            //else
            //{
            //    #region Fill City Drop-down
            //    var tempCity = _db.Cities.Where(m => m.DeletedAt == null).ToList();
            //    foreach (var city in tempCity)
            //    {
            //        model.cities.Add(new SelectListItem() { Text = city.Name, Value = city.CityId.ToString() });
            //    }
            //    #endregion Fill City Drop-down
            //}

            // Convert model.cities to IEnumerable<SelectListItem>
            var cities = listCity.Select(c => new { value = c.Value, text = c.Text }).ToList();

            return Json(cities);
        }
        public PartialViewResult Filter(List<int>? CountryId, List<int>? CityId, List<int>? ThemeId, List<int>? SkillId, string? searchText, int? sort, string pg = "")
        {
            var userId = HttpContext.Session.GetString("UserId");
            int u_id = int.Parse(userId);
            var cardData = new List<MissionCardModel>();
            var tempMission = new List<Mission>();
            if (pg == default)
            {
                pg = "1";
            }

            #region Search
            if (CountryId.Count==0&& CityId.Count==0&& ThemeId.Count==0)
            {
                if (searchText != null)
                {
                    var missions = _db.Missions.Where(x => x.DeletedAt == null && x.Status == true && x.Title.Contains(searchText)).ToList();
                    foreach (var m in missions)
                    {
                        bool t = tempMission.Any(x => x.MissionId == m.MissionId);
                        if (t == false)
                        {
                            tempMission.Add(m);
                        }
                    }
                }
            }
            

            #endregion Search

            #region Filter Country
            if (CountryId.Count != 0)
            {
                foreach (var country in CountryId)
                {
                    List<Mission> countries;
                    if (searchText != null)
                    {
                         countries = _db.Missions.Where(x => x.DeletedAt == null && x.Status == true && x.CountryId == country && x.Title.Contains(searchText)).ToList();
                    }
                    else
                    {
                         countries = _db.Missions.Where(x => x.DeletedAt == null && x.Status == true && x.CountryId == country).ToList();
                    }
                    foreach (var c in countries)
                    {
                        bool temp = tempMission.Any(x => x.MissionId == c.MissionId);
                        if (temp == false)
                        {
                            tempMission.Add(c);
                        }
                    }
                    
                }
            }
            #endregion Filter Country

            #region Filter City
            if (CityId.Count != 0)
            {
                foreach (var city in CityId)
                {
                    List<Mission> cities;
                    if (searchText != null)
                    {
                        cities = _db.Missions.Where(x => x.DeletedAt == null && x.Status == true && x.CityId == city && x.Title.Contains(searchText)).ToList();
                    }
                    else
                    {
                         cities = _db.Missions.Where(x => x.DeletedAt == null && x.Status == true && x.CityId == city).ToList();
                    }
                    foreach (var c in cities)
                    {
                        bool temp = tempMission.Any(x => x.MissionId == c.MissionId);
                        if (temp == false)
                        {
                            tempMission.Add(c);
                        }
                    }
                   
                }
            }
            #endregion Filter City

            #region Filter Themes
            if (ThemeId.Count != 0)
            {
                foreach (var theme in ThemeId)
                {
                    var themes = _db.Missions.Where(x => x.DeletedAt == null && x.Status == true && x.ThemeId == theme).ToList();
                    foreach (var t in themes)
                    {
                        bool temp = tempMission.Any(x => x.MissionId == t.MissionId);
                        if (temp == false)
                        {
                            tempMission.Add(t);
                        }
                    }
                }
            }
            #endregion Filter Country

            #region Filter skills
            if (SkillId.Count != 0)
            {
                foreach (var skill in SkillId)
                {
                    var themes = _db.MissionSkills.FirstOrDefault(ms => ms.SkillId == skill);
                    var missions = _db.Missions.Where(x => x.DeletedAt == null && x.Status == true && x.MissionId == themes.MissionId).ToList();
                    foreach (var t in missions)
                    {
                        bool temp = tempMission.Any(x => x.MissionId == t.MissionId);
                        if (temp == false)
                        {
                            tempMission.Add(t);
                        }
                    }
                }
            }
            #endregion Filter Country




            #region Combined Missions

            var missionsToAdd = new List<Mission>();

            if (CountryId.Count > 0 && CityId.Count > 0 && ThemeId.Count == 0)
            {
                foreach (int country in CountryId)
                {
                    foreach (int city in CityId)
                    {
                        var countriesANDcities = tempMission.Where(x => x.DeletedAt == null && x.Status == true && x.CountryId == country && x.CityId == city).ToList();
                        foreach (var c in countriesANDcities)
                        {
                            bool temp = missionsToAdd.Any(x => x.MissionId == c.MissionId);
                            if (temp == false)
                            {
                                missionsToAdd.Add(c);
                            }
                        }
                    }
                }
            }
            else if (CountryId.Count == 0 && CityId.Count > 0 && ThemeId.Count > 0)
            {
                foreach (int theme in ThemeId)
                {
                    foreach (int city in CityId)
                    {
                        var themesANDcities = tempMission.Where(x => x.DeletedAt == null && x.Status == true && x.ThemeId == theme && x.CityId == city).ToList();
                        foreach (var t in themesANDcities)
                        {
                            bool temp = missionsToAdd.Any(x => x.MissionId == t.MissionId);
                            if (temp == false)
                            {
                                missionsToAdd.Add(t);
                            }
                        }
                    }
                }
            }
            else if (CountryId.Count > 0 && CityId.Count == 0 && ThemeId.Count > 0)
            {
                foreach (int country in CountryId)
                {
                    foreach (int theme in ThemeId)
                    {
                        var themesANDcountries = tempMission.Where(x => x.DeletedAt == null && x.Status == true && x.ThemeId == theme && x.CountryId == country ).ToList();
                        foreach (var t in themesANDcountries)
                        {
                            bool temp = missionsToAdd.Any(x => x.MissionId == t.MissionId);
                            if (temp == false)
                            {
                                missionsToAdd.Add(t);
                            }
                        }
                    }
                }
            }
            else if (CountryId.Count > 0 && CityId.Count > 0 && ThemeId.Count > 0)
            {
                foreach (int country in CountryId)
                {
                    foreach (int city in CityId)
                    {
                        foreach (int theme in ThemeId)
                        {
                            var themesANDcountries = tempMission.Where(x => x.DeletedAt == null && x.Status == true && x.ThemeId == theme && x.CountryId == country && x.CityId == city).ToList();
                            foreach (var t in themesANDcountries)
                            {
                                bool temp = missionsToAdd.Any(x => x.MissionId == t.MissionId);
                                if (temp == false)
                                {
                                    missionsToAdd.Add(t);
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region Default Mission
            if (CountryId.Count == 0 && CityId.Count == 0 && ThemeId.Count == 0 && SkillId.Count == 0 && searchText == null )
            {
                tempMission = _db.Missions.Where(x => x.DeletedAt == null && x.Status == true).ToList();
            }
            #endregion Default Mission

            #region Create Card
            if ((CountryId.Count > 0 && CityId.Count > 0 && ThemeId.Count == 0) ||
               (CountryId.Count == 0 && CityId.Count > 0 && ThemeId.Count > 0) ||
               (CountryId.Count > 0 && CityId.Count == 0 && ThemeId.Count > 0) ||
               (CountryId.Count > 0 && CityId.Count > 0 && ThemeId.Count > 0))
            {
                foreach (var item in missionsToAdd)
                {
                    cardData.Add(CreateCard(item));
                }
            }
            else {
                foreach (var item in tempMission)
                {
                    cardData.Add(CreateCard(item));
                }
            }
            
            #endregion Create Card

            #region Sort Data
            if (sort != null)
            {
                switch (sort)
                {
                    case 1:
                        cardData = cardData.OrderByDescending(x => x.mission.CreatedAt).ToList();
                        break;
                    case 2:
                        cardData = cardData.OrderBy(x => x.mission.CreatedAt).ToList();
                        break;
                    case 3:
                        cardData = cardData.OrderBy(x => x.seatsLeft).ToList();
                        break;
                    case 4:
                        cardData = cardData.OrderByDescending(x => x.seatsLeft).ToList();
                        break;
                    case 5:
                        cardData = cardData.Where(x=>x.favMission==1).ToList();
                        break;
                    case 6:
                        cardData = cardData.OrderByDescending(x => x.mission.EndDate).ToList();
                        break;
                }
            }
            #endregion Sort Data


           
          

            const int pageSize = 9;
            int recsCount = tempMission.Count();
            var pager = new Pager(recsCount, int.Parse(pg), pageSize);
            int recSkip = (int.Parse(pg) - 1) * pageSize;
           // MailViewModel listOfMail = _objUserInterface.fillMailmodel();
           //ViewBag.Mail = listOfMail;
            List<User> userlist = _objILogin.Users(u_id);
            
            ViewBag.Users = userlist;
            // replace with your logic to get the users

           

            var data = cardData.Skip(recSkip).Take(pager.PageSize).ToList();
            cardData = data;
            this.ViewBag.Pager = pager;

            int tempMissionCount;
            if ((CountryId.Count > 0 && CityId.Count > 0 && ThemeId.Count == 0) ||
               (CountryId.Count == 0 && CityId.Count > 0 && ThemeId.Count > 0) ||
               (CountryId.Count > 0 && CityId.Count == 0 && ThemeId.Count > 0) ||
               (CountryId.Count > 0 && CityId.Count > 0 && ThemeId.Count > 0))
            {
                tempMissionCount = missionsToAdd.Count(); 
            }
            else
            {
                tempMissionCount = tempMission.Count();
            }

            this.ViewBag.count = tempMissionCount;
            

            return PartialView("_MissionGridPartial", cardData);
        }


        #region Create Card
        public MissionCardModel CreateCard(Mission mission)
        {
            var userId = HttpContext.Session.GetString("UserId");
            int u_id = int.Parse(userId);
            var card = new MissionCardModel();
            card.mission = mission;
            var img = _db.MissionMedia.FirstOrDefault(x => x.MissionId == mission.MissionId);
            card.CardImg = img != null ? img.MediaPath : "~/images/Grow-Trees-On-the-path-to-environment-sustainability-1.png";

            card.alreadyVolunteered = (_db.MissionApplications.Where(x => x.MissionId == mission.MissionId && x.ApprovalStatus=="ACCEPT").Count());
            if (int.Parse(mission.SeatAvailable) > 0)
            {
                card.seatsLeft = (int)(int.Parse(mission.SeatAvailable) - card.alreadyVolunteered);
            }
            card.goalMission = _db.GoalMissions.FirstOrDefault(x => x.MissionId == mission.MissionId);
            if (card.goalMission != null)
            {
                float totalGoal = card.goalMission.GoalValue;
                card.progressBar = totalGoal;
            }
            card.missionApplied = _db.MissionApplications.FirstOrDefault(x => x.MissionId == mission.MissionId && x.UserId == u_id && x.ApprovalStatus=="ACCEPT") != null ? 1 : 0;
            card.approvalPending = _db.MissionApplications.FirstOrDefault(x => x.MissionId == mission.MissionId && x.UserId == u_id
&& x.ApprovalStatus == "PENDING") != null ? 1 : 0;
            var id = HttpContext.Session.GetString("UserId");
            if (id != null)
            {
                int uid = int.Parse(id);
                card.favMission = _db.FavouriteMissions.FirstOrDefault(x => x.MissionId == mission.MissionId && x.UserId == uid && x.DeletedAt == null) != null ? 1 : 0;
            }
            var ratings = _db.MissionRatings.Where(t => t.MissionId == mission.MissionId).ToList();
            float rating = 0;
            float sum = 0;
            float count = ratings.Count;
            foreach (var entry in ratings)
            {
                sum = sum + int.Parse(entry.Rating);

            }
            rating = sum / count;
            if (rating != null)
            {
                card.avgRating = rating;
            }

            card.theme = _db.MissionThemes.FirstOrDefault(x => x.MissionThemeId == mission.ThemeId).Title;
            card.country = _db.Cities.FirstOrDefault(x => x.CityId == mission.CityId).Name;

            
            return card;
        }
        #endregion Create Card

        #region Apply Mission
        [HttpPost]
        public JsonResult ApplyMission(int id)
        {
            var u_id = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(u_id))
            {
                return Json("You have to login first");
            }
            if (_db.MissionApplications.FirstOrDefault(x => x.MissionId == id && x.UserId == int.Parse(u_id) && x.ApprovalStatus == "ACCEPT") != null)
            {
                return Json("You are already Part of mission");
            }
            else if (_db.MissionApplications.FirstOrDefault(x => x.MissionId == id && x.UserId == int.Parse(u_id)) != null)
            {
                return Json("You already applied in this mission... wait foe approval.");
            }
            else
            {
                var missionApplication = new MissionApplication();
                missionApplication.MissionId = id;
                missionApplication.UserId = int.Parse(u_id);
                missionApplication.ApprovalStatus = "PENDING";
                missionApplication.AppliedAt = DateTime.Now;
                _db.MissionApplications.Add(missionApplication);
                _db.SaveChanges();
                return Json("Applied Sucessfully");
            }
        }
        #endregion Apply Mission


        public IActionResult VolunteeringMissionPage()
        {
            return View();
        }


    }
}