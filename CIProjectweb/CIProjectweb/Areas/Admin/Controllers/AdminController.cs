
using CIProjectweb.Entities.AdminViewModel;
using CIProjectweb.Entities.DataModels;
using CIProjectweb.Entities.ViewModels;

using CIProjectweb.Repository.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
namespace CIProjectweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdmin _objIAdmin;
       

        private readonly ILogger<AdminController> _logger;
        private readonly IToastNotification _toastNotification;

        public AdminController(ILogger<AdminController> logger, IAdmin objIAdmin, IToastNotification toastNotification)
        {
            _logger = logger;
            _objIAdmin = objIAdmin;
            _toastNotification = toastNotification;

        }
       

        public IActionResult Index(long id)
        {
            HttpContext.Session.SetInt32("Nav", 1);
            ViewBag.nav = HttpContext.Session.GetInt32("Nav");
           
            List<User> users = _objIAdmin.alluser();
            List<City> city= _objIAdmin.cities();
            List<Country> country = _objIAdmin.countries();
            ViewBag.country = country;
            ViewBag.city = city;
            return View(users);
           
        }

        [HttpPost]
        public IActionResult AddUser(string FirstName, string Lastname,string Avtar,string Email,string EmployeeId,string Password,string Department,long City,long Country,string Profile,string Status,long UserId)
        {
            bool success = _objIAdmin.AddUser(FirstName,Lastname,Email,EmployeeId, Password,Department, City,Country,Profile,Status,Avtar, UserId);
            return Json(new { success = success });

        }
        public IActionResult Application()
        {
            HttpContext.Session.SetInt32("Nav", 6);
            ViewBag.nav = HttpContext.Session.GetInt32("Nav");
            List<MissionApplicationView> missionApplicationView = new List<MissionApplicationView>();
            List<MissionApplication> missionApplications = _objIAdmin.allmissionapp();
            List<Mission> missions = _objIAdmin.allmission();
            List<User> users = _objIAdmin.alluser();
            var missionapp = (from ma in missionApplications join ms in missions on ma.MissionId equals ms.MissionId join u in users on ma.UserId equals u.UserId select new { Title = ms.Title, MissionId = ma.MissionId,applicationID = ma.MissionApplicationId, UserId = ma.UserId, FirstName = u.FirstName, LastName = u.LastName, AppliedAt = ma.AppliedAt }).ToList();
            foreach (var item in missionapp)
            {
                MissionApplicationView missionApplicationView1 = new MissionApplicationView();
                missionApplicationView1.Title = item.Title;
                missionApplicationView1.applicationId = item.applicationID;
                missionApplicationView1.MissionId = item.MissionId;
                missionApplicationView1.UserId = item.UserId;
                missionApplicationView1.FirstName = item.FirstName;
                missionApplicationView1.LastName = item.LastName;
                missionApplicationView1.AppliedAt = item.AppliedAt;
                missionApplicationView.Add(missionApplicationView1);
            }
            return View(missionApplicationView);
        }
        public IActionResult Story()
        {
            HttpContext.Session.SetInt32("Nav", 7);
            ViewBag.nav = HttpContext.Session.GetInt32("Nav");
            List<StoryView> storyViews = new List<StoryView>();
            List<Mission> missions = _objIAdmin.allmission();
            List<User> users = _objIAdmin.alluser();
            List<Story> stories = _objIAdmin.allstory();
            var storyrecord = (from st in stories join u in users on st.UserId equals u.UserId join ms in missions on st.MissionId equals ms.MissionId select new { StoryTitle = st.Title, FirstName = u.FirstName, LastName = u.LastName, MissionTitle = ms.Title, MissionId = st.MissionId, UserId = st.UserId, StoryId = st.StoryId }).ToList();
            foreach (var item in storyrecord)
            {
                StoryView storyView = new StoryView();
                storyView.MissionTitle = item.MissionTitle;
                storyView.StoryTitle = item.StoryTitle;
                storyView.StoryId = item.StoryId;
                storyView.UserId = item.UserId;
                storyView.FirstName = item.FirstName;
                storyView.LastName = item.LastName;
                storyViews.Add(storyView);
            }
            return View(storyViews);
        }
        [HttpPost]
        public IActionResult GetUsers()
        {
            return PartialView("_User1");

        }
        [HttpPost]
        public IActionResult UserDelete(long userId)
        {
            _objIAdmin.deleteuser(userId);
            return Json(null);
        }
        [HttpPost]
        public IActionResult Decline(long applicationid)
        {
            bool success = _objIAdmin.Decline(applicationid);
            return Json(new { success = success });
        }
        [HttpPost]
        public IActionResult CMSDELETE(long PageId)
        {
            _objIAdmin.deleteCMS(PageId);
            return Json(null);
        }
        public IActionResult Theme()
        {
            if (TempData.ContainsKey("Message"))
            {
                string msg = (string)TempData["Message"];
                if (msg == "Successfully  Added!")
                {
                    // Success Toast
                    _toastNotification.AddSuccessToastMessage(msg);
                }
                else
                {
                    // Success Toast
                    _toastNotification.AddSuccessToastMessage(msg);
                }

            }
            HttpContext.Session.SetInt32("Nav", 4);
            ViewBag.nav = HttpContext.Session.GetInt32("Nav");
            List<MissionTheme> missionThemes = _objIAdmin.alltheme();
            ThemeView themeView = new ThemeView();
            themeView.missionThemes = missionThemes;
            return View(themeView);
        }
        [HttpPost]
        public IActionResult Theme(ThemeView themeView)
        {
            bool success = _objIAdmin.addtheme(themeView); 
            if (!success)
            {

                TempData["Message"] = "Update Successfully!";


                return RedirectToAction("Theme", "Admin");
            }
            else
            {
                TempData["Message"] = "Successfully  Added!";
                return RedirectToAction("Theme", "Admin");
            }

            
            
        }
        [HttpPost]
        public IActionResult ThemeEdit(long themeId)
        {
            var theme = _objIAdmin.themeedit(themeId);
            return Json(theme);
        }
        [HttpPost]
        public IActionResult ThemeDelete(long themeId)
        {
            _objIAdmin.deletetheme(themeId);
            return Json(null);
        }
        [HttpPost]
        public IActionResult ApproveStory(long storyId)
        {
            _objIAdmin.approve(storyId);
            return Json(null);
        }
        [HttpPost]
        public IActionResult RejectStory(long storyId)
        {
            _objIAdmin.reject(storyId);
            return Json(null);
        }
        [HttpPost]
        public IActionResult DeleteStory(long storyId)
        {
            _objIAdmin.delete(storyId);
            return Json(null);
        }
        [HttpPost]
        public IActionResult Approve(long applicationid)
        {
            bool success=_objIAdmin.Approve(applicationid);
            return Json(new {success=success});
        }
        [HttpPost]
        public IActionResult AddCMS(CMSViewModel cmsadd)
        {
            bool success=_objIAdmin.ADDCms(cmsadd);
            if (!success)
            {

                TempData["Message"] = "Update Successfully!";


                return RedirectToAction("CMS", "Admin"); 
            }
            else
            {
                TempData["Message"] = "Successfully  Added!";
                return RedirectToAction("CMS", "Admin");
            }
            
        }
        [HttpPost]
        public IActionResult EditCMS(long CMSID)
        {
            CmsPage cmsedit = _objIAdmin.EditCMS(CMSID);
            return Json(cmsedit);

        }
        [HttpPost]
        public IActionResult EditUser(long UserId)
        {
            User user=_objIAdmin.EditUser(UserId);
            return Json(user);
        }
       
        public IActionResult Mission()
        {
            HttpContext.Session.SetInt32("Nav", 2);
            ViewBag.nav = HttpContext.Session.GetInt32("Nav");
            MissionView missionView = new MissionView();
            List<Mission> missions = _objIAdmin.allmission();
            List<MissionTheme> missionThemes = _objIAdmin.alltheme();
            List<Country> countries = _objIAdmin.countries();
            List<City> citys = _objIAdmin.cities();
            List<Skill> skills = _objIAdmin.skilllist();
            List<GoalMission> goalMissions = _objIAdmin.allgoalmissions();
            missionView.goalmissions = goalMissions;
            missionView.missions = missions;
            missionView.missionThemes = missionThemes;
            missionView.countries = countries;
            missionView.citys = citys;

            missionView.skills = skills;
            return View(missionView);
        }
        [HttpPost]
        public IActionResult Mission(MissionView missionView, string[] selectedValues, string[] dataUrls, string[] docFiles, string[] docName, string videoUrls)
        {
            _objIAdmin.savemission(missionView, selectedValues, dataUrls, docFiles, docName, videoUrls);
            return Json(null);
        }
        public IActionResult CMS()
        {
            if (TempData.ContainsKey("Message"))
            {
                string msg= (string)TempData["Message"];
                if(msg == "Successfully  Added!")
                {
                    // Success Toast
                    _toastNotification.AddSuccessToastMessage(msg);
                }
                else
                {
                    // Success Toast
                    _toastNotification.AddSuccessToastMessage(msg);
                }
              
            }
            HttpContext.Session.SetInt32("Nav", 3);
            ViewBag.nav = HttpContext.Session.GetInt32("Nav");
            CMSViewModel cmsPages = _objIAdmin.cmsrecordall();
            return View(cmsPages);
        }
        public IActionResult Banner()
        {
            if (TempData.ContainsKey("Message"))
            {
                string msg = (string)TempData["Message"];
                if (msg == "Successfully  Added!")
                {
                    // Success Toast
                    _toastNotification.AddSuccessToastMessage(msg);
                }
                else
                {
                    // Success Toast
                    _toastNotification.AddSuccessToastMessage(msg);
                }

            }
            HttpContext.Session.SetInt32("Nav", 8);
            ViewBag.nav = HttpContext.Session.GetInt32("Nav");
            List<Banner> bannerViews2 = _objIAdmin.bannerlist();
            BannerView bannerView = new BannerView();
            bannerView.banners = bannerViews2;
            return View(bannerView);
        }
        [HttpPost]
        public IActionResult Banner(BannerView bannerView)
        {
            bool success = _objIAdmin.bannersave(bannerView); 
            if (!success)
            {

                TempData["Message"] = "Update Successfully!";


                return RedirectToAction("Banner", "Admin");
            }
            else
            {
                TempData["Message"] = "Successfully  Added!";
                return RedirectToAction("Banner", "Admin");
            }
            
            
        }
        [HttpPost]
        public IActionResult BannerEdit(long bannerId)
        {
            var banner = _objIAdmin.banner(bannerId);
            return Json(banner);
        }
        [HttpPost]
        public IActionResult BannerDelete(long bannerId)
        {
            _objIAdmin.deletebanner(bannerId);
            return Json(null);
        }
        public IActionResult Skill()
        {
            HttpContext.Session.SetInt32("Nav", 5);
            ViewBag.nav = HttpContext.Session.GetInt32("Nav");
            List<Skill> skills = _objIAdmin.skilllist();
            SkillView skillView = new SkillView();
            skillView.skills = skills;
            return View(skillView);
        }
        [HttpPost]
        public IActionResult Skill(SkillView skillView)
        {
            _objIAdmin.skillsave(skillView);
            return RedirectToAction("Skill", "Admin");
        }
        [HttpPost]
        public IActionResult SkillEdit(long skillId)
        {
            var skill = _objIAdmin.skilledit(skillId);
            return Json(skill);
        }
        [HttpPost]
        public IActionResult SkillDelete(long skillId)
        {
            bool success = _objIAdmin.deleteskill(skillId);
            return Json(new { success = success });
            
        }
        #region CountryByCityAdmin

        public IActionResult CountryByCityAdmin(long CountryId)
        {
            var countryByCityAdmin = _objIAdmin.CountryByCityAdmin(CountryId);
            return Json(new { data = countryByCityAdmin });
        }

        #endregion
        [HttpPost]
        public IActionResult MissionEdit(long missionId)
        {
            var mission = _objIAdmin.findmission(missionId);
            List<MissionSkill> missionSkills = mission.missionSkill;
            var skillId = missionSkills?.Select(ms => ms.SkillId).ToArray();
            List<MissionMedium> missionMedia = mission.missionMedia;
            var missionUrl = missionMedia?.Select(mm => new { MediaPath = mm.MediaPath, MediaType = mm.MediaType }).ToArray();
            List<MissionDocument> missionDocuments = mission.missionDocuments;
            var missiondoc = missionDocuments?.Select(md => new { DocName = md.DocumentName, DocPath = md.DocumentPath }).ToArray();
            return Json(new { mission = mission, missionSkillId = skillId, missionUrl = missionUrl, missiondoc = missiondoc });
        }
        [HttpPost]
        public IActionResult MissionDelete(long missionId)
        {
            _objIAdmin.deletemission(missionId);
            return Json(null);
        }
    }
}
