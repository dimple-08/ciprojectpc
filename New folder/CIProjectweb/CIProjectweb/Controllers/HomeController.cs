using CIProjectweb.Entities.ViewModels;
using CIProjectweb.Models;
using CIProjectweb.Repository.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;

using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using CIProjectweb.Entities.DataModels;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CIProjectweb.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogin _objILogin; 
        private readonly IUserInterface _objUserInterface;
        private readonly ILogger<HomeController> _logger;
        private readonly CIDbContext _db;

        public HomeController(ILogger<HomeController> logger,ILogin objLogin, CIDbContext db, IUserInterface objUserInterface)
        {
            _logger = logger;
            _objILogin = objLogin;
            _objUserInterface = objUserInterface;
            _db = db;
        }

        public IActionResult Index()
        {
            List<MissionViewModel> missionlist = _objUserInterface.getmissions(10003);
            string jsonData = JsonSerializer.Serialize(missionlist);

             ViewBag.JsonData = jsonData;
            
            return View();

         

        }

        public IActionResult Privacy(string slug)
        {
            List<CmsPage> list = _db.CmsPages.Where(cms => cms.Slug == slug && cms.DeletedAt==null && cms.Status==true).ToList();
            ViewBag.slugs = _db.CmsPages.Where(m => m.DeletedAt == null).ToList();

            return View(list);
        }




       

        public IActionResult Login()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.AlertMessage = TempData["Message"];
            }

            return View();
        }
        #region LoadBanner

        [HttpPost]

        public IActionResult LoadBanner()
        {
            var banner =_objUserInterface.LoadBannerGet();
            return Json(new { data = banner });
        }

        #endregion
        #region Login
        [HttpPost]
        public IActionResult Login(LoginViewModel objlogin)
        {
            if (ModelState.IsValid)
            {
               
                int validate = _objILogin.validateUser(objlogin);
                HttpContext.Session.SetString("UserId", validate.ToString());
                var userId = HttpContext.Session.GetString("UserId");
                int u_id = int.Parse(userId);
                User user = _objILogin.GetUsers(u_id);
                var name = _objILogin.getUserName(objlogin);
                if (name != null)
                {
                    HttpContext.Session.SetString("UserName", name);
                }
                if (validate!=0)
                {
                    var identity = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name,objlogin.Email)},CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal=new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
                    HttpContext.Session.SetString("Avtar", user.Avatar == null ? "/images/user1.png" : user.Avatar);
                    HttpContext.Session.SetString("Email", user.Email == null ? "Please provide email" : user.Email);
                    // Generate URL for the Index action in the Admin area
                    string areaUrl = Url.Action("Index", "Admin", new { area = "Admin",id=user.UserId });

                    // Redirect to the URL
                    
                    if (user.Role == "Admin")
                    {
                        HttpContext.Session.SetString("User", name);
                        return Redirect(areaUrl);

                    }
                    else
                    {
                        return RedirectToAction("LandingPage", "Mission");
                    }
                    //Session["UserName"] = listofuser.username.ToString();
                   
                }
                else
                {
                    if (user==null)
                    {
                        ModelState.AddModelError("Email", "User don't exists.");
                        return View(objlogin);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Your credentials are wrong");
                        return View(objlogin);
                    }
                   
                }

            }
            else
            {
                return View();
            }
            return View();
        }
        #endregion

        #region LogOut
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Avtar");
            var storecookies = Request.Cookies.Keys;
            foreach(var cookie in storecookies)
            {
                Response.Cookies.Delete(cookie);
                      
            }
            return RedirectToAction("Login");
        }
        #endregion

        public IActionResult ForgotPAasword()
        {
            return View();
        }
        #region forgot Password post
        [HttpPost]
        public IActionResult ForgotPAasword(ForgotPasswordViewModel objFpvm)
        {
            if (ModelState.IsValid)
            {
                var Email = objFpvm.Email;
                
                var token = Guid.NewGuid().ToString();
                bool usercheck = _objUserInterface.ValideUserEmail(objFpvm,token);
                if (usercheck)
                {
                    
                    // Send an email with the password reset link to the user's email address
                    var resetLink = Url.Action("ResetPassword", "Home", new { email = Email , token }, Request.Scheme);
                    // Send email to user with reset password link
                    // ...
                    var fromAddress = new MailAddress("gajeravirajpareshbhai@gmail.com", "Sender Name");
                    var toAddress = new MailAddress(objFpvm.Email);
                    var subject = "Password reset request";
                    var body = $"Hi,<br /><br />Please click on the following link to reset your password:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
                    var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("bhavsardimple7@gmail.com", "ibjlmmwrsmhvtbeh"),
                        EnableSsl = true
                    };
                    smtpClient.Send(message);
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("Email", "User don't Exists");
                    return View(objFpvm);
                }
            }
            return View();
        }
        #endregion
        public IActionResult registration()
        {
            return View();
        }

        #region registartion post
        [HttpPost]
        public IActionResult registration(RegistrationViewModel objredistervm)
        {
            

            if (ModelState.IsValid)
            {
                bool addUser = _objUserInterface.AddUser(objredistervm);
                if (addUser)
                {

                    TempData["Message"] = "Successfully Registered !!";


                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ModelState.AddModelError("Email", "User with this E-mail Exsits");
                    return View(objredistervm);
                }
            }
            return View(objredistervm);
        }
        #endregion

        #region Reset Password get
        [HttpGet]
        public IActionResult ResetPAssword(string email, string token)
        {
            
            var model = new ResetPAsswordViewModel
                {
                    Email = email,
                    Token = token,

                };
                return View(model);
            




        }
        #endregion

        #region ResetPassword Post
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPAssword(ResetPAsswordViewModel resetvm)
        {
            bool success_ = _objUserInterface.ResetPassword(resetvm.Email, resetvm.Token);
            if (success_)
            {
                bool update = _objUserInterface.updatePassword(resetvm);


                TempData["Message"] = "Successfully Changed !!";


                return RedirectToAction("Login", "Home");


            }
            else
            {
                ViewBag.Message = "Something went horribly wrong!";
               
            }
            return View();
        }

        #endregion
        public IActionResult LandingPage()
        {
            //var userId = HttpContext.Session.GetString("UserId");
            ////if (userId == null)
            ////{
            ////    return RedirectToAction("Login");
            ////}
            //int u_id = int.Parse(userId);
           
            var userId = HttpContext.Session.GetString("UserId");
            
            if (userId!=null)
            {
                int u_id = int.Parse(userId);
                LandingPAgeViewModel datauser = _objUserInterface.GetCustomers(1, u_id);
                List<MissionViewModel> missionlistuser = _objUserInterface.getmissions(u_id);
                List<CountryViewModel> countryuser = _objUserInterface.CountryList();
                List<CityViewModel> cityuser = _objUserInterface.CityList();
                List<ThemeViewModel> themeuser = _objUserInterface.ThemeList();
                string jsonCityuser = JsonSerializer.Serialize(cityuser);
                string jsonContryuser = JsonSerializer.Serialize(countryuser);
                string jsonDatauser = JsonSerializer.Serialize(missionlistuser);
                string jsonThemeuser = JsonSerializer.Serialize(themeuser);
                ViewBag.Country = jsonContryuser;
                ViewBag.City = jsonCityuser;
                ViewBag.Theme = jsonThemeuser;
                ViewBag.JsonData = jsonDatauser;
                int countlist = missionlistuser.Count();
                ViewBag.Count = countlist;
                return View(datauser);

            }
            else {
                LandingPAgeViewModel data = _objUserInterface.GetCustomers(1, 10003);
                List<MissionViewModel> missionlist = _objUserInterface.getmissions();
                List<CountryViewModel> country = _objUserInterface.CountryList();
                List<CityViewModel> city = _objUserInterface.CityList();
                List<ThemeViewModel> theme = _objUserInterface.ThemeList();
                string jsonCity = JsonSerializer.Serialize(city);
                string jsonContry = JsonSerializer.Serialize(country);
                string jsonData = JsonSerializer.Serialize(missionlist);
                string jsonTheme = JsonSerializer.Serialize(theme);
                ViewBag.Country = jsonContry;
                ViewBag.City = jsonCity;
                ViewBag.Theme = jsonTheme;
                ViewBag.JsonData = jsonData;
                int count = missionlist.Count();
                ViewBag.Count = count;
                return View(data);

            }
            
        }
        [HttpPost]
        public IActionResult LandingPage(int currentPageIndex)
        {
           
            return RedirectToAction("LandingPage", "Mission", new {pg=1});
        }

        [HttpPost]
        public JsonResult SearchMission(string missionName)
        {
            
            
            List<Mission> missionlist = _objUserInterface.getmission(missionName);
            
            return Json(missionlist.ToList().Take(6));
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }

        #region RelatedMission
        public IActionResult RelatedMissionPage(int id,int? usersid,int? pageIndex=1)
        {
           

            if (usersid!=null)
            {
                int u_id = (int)usersid;
                MissionViewModel mission = _objUserInterface.getmission(id, u_id);
                List<MissionViewModel> relatedmission = _objUserInterface.getrelatedmissions(id,u_id);
                List<CommentViewModel> commentList = _objUserInterface.getcomment(id);
                List<User> userlist = _objILogin.Users(u_id);
                User user = _objILogin.GetUsers(u_id);
                var temp = _db.MissionApplications.Where(x => x.MissionId == id ).ToList();
                var listVolunteering = new List<RecentVolunteerVM>();
                foreach (var u in temp)
                {
                    var volunteeringModel = new RecentVolunteerVM();
                    var user1 = _db.Users.FirstOrDefault(x => x.UserId == u.UserId);
                    volunteeringModel.missions = user1.FirstName + " " + user1.LastName;
                    volunteeringModel.users = user1.Avatar != null ? user1.Avatar : "/images/user1.png";
                    listVolunteering.Add(volunteeringModel);
                }
               
                List<MissionDocument> missiondocument = _objUserInterface.getmissionDocument(id);
                ViewBag.comment = commentList;
                ViewBag.recent= listVolunteering;
                ViewBag.Users = userlist;
                ViewBag.user=user;
                ViewBag.documents=missiondocument;
                ViewBag.RelatedMission = relatedmission;
                ViewBag.slugs = _db.CmsPages.Where(m => m.DeletedAt == null).ToList();
                int pageSize = 2; // Set the page size to 9
                var volunteers = listVolunteering; // Retrieve all volunteers from data source
                int totalCount = volunteers.Count(); // Get the total number of volunteers
                int skip = (int)((pageIndex - 1) * pageSize);
                var volunteersOnPage = volunteers.Skip(skip).Take(pageSize).ToList(); // Get the volunteers for the current page
                ViewBag.count = volunteersOnPage.Count;
                ViewBag.skip = (skip + 1);
                ViewBag.TotalCount = totalCount;
                ViewBag.PageSize = pageSize;
                ViewBag.PageIndex = pageIndex;
                ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                ViewBag.RecentVolunteers = volunteersOnPage;
                return View(mission);
            }
            var userid = HttpContext.Session.GetString("UserId");
            if (userid != null)
            {
                int u_id = int.Parse(userid);
                MissionViewModel mission = _objUserInterface.getmission(id, u_id);
                List<MissionViewModel> relatedmission = _objUserInterface.getrelatedmissions(id, u_id);
                List<CommentViewModel> commentList = _objUserInterface.getcomment(id);
                List<User> userlist = _objILogin.Users(u_id);
                User user = _objILogin.GetUsers(u_id);
                List<MissionDocument> missiondocument = _objUserInterface.getmissionDocument(id);
                var temp = _db.MissionApplications.Where(x => x.MissionId == id).ToList();
                var listVolunteering = new List<RecentVolunteerVM>();
                foreach (var u in temp)
                {
                    var volunteeringModel = new RecentVolunteerVM();
                    var user1 = _db.Users.FirstOrDefault(x => x.UserId == u.UserId);
                    volunteeringModel.missions = user1.FirstName + " " + user1.LastName;
                    volunteeringModel.users = user1.Avatar != null ? user1.Avatar : "/images/user1.png";
                    listVolunteering.Add(volunteeringModel);
                }
                ViewBag.user = user;
                ViewBag.recent = listVolunteering;
                ViewBag.documents = missiondocument;
                ViewBag.comment = commentList;
                ViewBag.Users=userlist;
                ViewBag.slugs = _db.CmsPages.Where(m => m.DeletedAt == null).ToList();
                ViewBag.RelatedMission = relatedmission;
                int pageSize = 2; // Set the page size to 9
                var volunteers = listVolunteering; // Retrieve all volunteers from data source
                int totalCount = volunteers.Count(); // Get the total number of volunteers
                int skip = (int)((pageIndex - 1) * pageSize);
                var volunteersOnPage = volunteers.Skip(skip).Take(pageSize).ToList(); // Get the volunteers for the current page
                ViewBag.count=volunteersOnPage.Count;
                ViewBag.skip = (skip + 1);
                ViewBag.TotalCount = totalCount;
                ViewBag.PageSize = pageSize;
                ViewBag.PageIndex = pageIndex;
                ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                ViewBag.RecentVolunteers = volunteersOnPage;
                return View(mission);
            }
            else
            {
                int u_id = int.Parse(userid);
                MissionViewModel mission = _objUserInterface.getmission(id);
                List<MissionViewModel> relatedmission = _objUserInterface.getrelatedmissions(id, u_id);
                List<CommentViewModel> commentList = _objUserInterface.getcomment(id);
                List<User> userlist = _objILogin.Users(u_id);
                List<MissionDocument> missiondocument = _objUserInterface.getmissionDocument(id);
                var temp = _db.MissionApplications.Where(x => x.MissionId == id).ToList();
                var listVolunteering = new List<RecentVolunteerVM>();
                foreach (var u in temp)
                {
                    var volunteeringModel = new RecentVolunteerVM();
                    var user1 = _db.Users.FirstOrDefault(x => x.UserId == u.UserId);
                    volunteeringModel.missions = user1.FirstName + " " + user1.LastName;
                    volunteeringModel.users = user1.Avatar != null ? user1.Avatar : "/images/user1.png";
                    listVolunteering.Add(volunteeringModel);
                }

                ViewBag.documents = missiondocument;
                ViewBag.recent = listVolunteering;
                ViewBag.Users = userlist;
                ViewBag.comment = commentList;
                ViewBag.slugs = _db.CmsPages.Where(m => m.DeletedAt == null).ToList();
                ViewBag.RelatedMission = relatedmission;
                int pageSize = 2; // Set the page size to 9
                var volunteers = listVolunteering; // Retrieve all volunteers from data source
                int totalCount = volunteers.Count(); // Get the total number of volunteers
                int skip = (int)((pageIndex - 1) * pageSize);
                var volunteersOnPage = volunteers.Skip(skip).Take(pageSize).ToList(); // Get the volunteers for the current page
                ViewBag.count = volunteersOnPage.Count;
                ViewBag.skip = (skip + 1);
                ViewBag.TotalCount = totalCount;
                ViewBag.PageSize = pageSize;
                ViewBag.PageIndex = pageIndex;
                ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                ViewBag.RecentVolunteers = volunteersOnPage;
                return View(mission);
            }
           
        }


        #endregion

        [HttpPost]
        public async Task<IActionResult> SendRecStory(long StoryId, string[] ToMail)
        {
            var u_id = HttpContext.Session.GetString("UserId");
            if (u_id != null)
            {
                int uIds = int.Parse(u_id);
                var FromUser = _objILogin.GetUsers(uIds);
                foreach (var item in ToMail)
                {
                    var uId = _objUserInterface.getuserEmail(item);

                    var resetLink = "https://localhost:44357" + Url.Action("StoryDetailPage", "Home", new { storyid = StoryId, usersid = uId.UserId });
                    var fromAddress = new MailAddress(FromUser.Email, FromUser.FirstName);
                    var toAddress = new MailAddress(item);
                    ;
                    var subject = "Message For Recommand Mission";
                    var body = $"Hi,<br /><br />Please click on the following link to reset your password:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
                    var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("bhavsardimple7@gmail.com", "ibjlmmwrsmhvtbeh"),
                        EnableSsl = true
                    };
                    smtpClient.Send(message);


                    StoryInvite storyInviteExists = _objUserInterface.storyInviteExists((int)FromUser.UserId, (int)uId.UserId, StoryId);
                    bool ADDInvite = _objUserInterface.ADDstoryInvite(storyInviteExists, (int)FromUser.UserId, (int)uId.UserId, StoryId);




                }

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

        }


        #region multiple mail


        [HttpPost]
        public async Task<IActionResult> SendRec(int missionId, string[] ToMail )
        {
            try
            {
                var u_id = HttpContext.Session.GetString("UserId");
                if (u_id != null)
                {
                    int uIds = int.Parse(u_id);
                    var FromUser = _objILogin.GetUsers(uIds);
                    foreach (var item in ToMail)
                    {
                        var uId = _objUserInterface.getuserEmail(item);

                        var resetLink = "https://localhost:44357" + Url.Action("RelatedMissionPage", "Home", new { id = missionId, usersid = uId.UserId });
                        var fromAddress = new MailAddress(FromUser.Email, FromUser.FirstName);
                        var toAddress = new MailAddress(item);
                        ;
                        var subject = "Message For Recommand Mission";
                        var body = $"Hi,<br /><br />Please click on the following link to reset your password:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
                        var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = true
                        };
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                        {
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential("bhavsardimple7@gmail.com", "ibjlmmwrsmhvtbeh"),
                            EnableSsl = true
                        };
                        smtpClient.Send(message);


                        MissionInvite missionInviteExists = _objUserInterface.missionInviteExists((int)FromUser.UserId, (int)uId.UserId, missionId);
                        bool ADDInvite = _objUserInterface.ADDMissionInvite(missionInviteExists, (int)FromUser.UserId, (int)uId.UserId, missionId);




                    }

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }

            }
            catch (Exception ex)
            {

                var errorViewModel = new ErrorViewModel
                {

                    RequestId = ex.ToString()
                };
                // Redirect to an error page or show a generic error message
                return RedirectToAction("Error", "Home");
            }


        }

        #endregion multiple mail

        #region RecomandUser

        [HttpPost]
        public IActionResult RecomandUser(string EmailId, int MissionId)
        {
            try
            {
                var u_id = HttpContext.Session.GetString("UserId");
                if (u_id != null)
                {
                    int uIds = int.Parse(u_id);
                    var FromUser = _objILogin.GetUsers(uIds);
                    var recomandUser = _objUserInterface.getuserEmail(EmailId);
                    if (recomandUser != null)
                    {


                        var resetLink = "https://localhost:44357" + Url.Action("RelatedMissionPage", "Home", new { id = MissionId, usersid = recomandUser.UserId });
                        var fromAddress = new MailAddress("bhavsardEmailIdimple7@gmail.com", "Dimple");
                        var toAddress = new MailAddress(EmailId)
            ;
                        var subject = "Message For Recommand Mission";
                        var body = $"Hi,<br /><br />Please click on the following link to reset your password:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
                        var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = true
                        };
                        var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                        {
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential("bhavsardimple7@gmail.com", "ibjlmmwrsmhvtbeh"),
                            EnableSsl = true
                        };
                        smtpClient.Send(message);
                        MissionInvite missionInviteExists = _objUserInterface.missionInviteExists((int)FromUser.UserId, (int)recomandUser.UserId, MissionId);
                        bool ADDInvite = _objUserInterface.ADDMissionInvite(missionInviteExists, (int)FromUser.UserId, (int)recomandUser.UserId, MissionId);
                        return Json(new { success = true });

                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
                else
                {
                    return Json(new { success = false });
                }

            }
            catch (Exception ex)
            {

                var errorViewModel = new ErrorViewModel
                {

                    RequestId = ex.ToString()
                };
                // Redirect to an error page or show a generic error message
                return RedirectToAction("Error", "Home");
            }
          
             
        }

        #endregion
        [HttpPost]
        public IActionResult Share_Story(string[] Image, int MissionId, string Title, DateTime Date, string Description, int UserId, string[] videoUrls,string value)
        {
            try
            {
                var story = _objUserInterface.getstory(Image, MissionId, Title, Date, Description, UserId, videoUrls, value);
                if (story != null && story.Status == "DRAFT")
                {
                    return Json(new { success = true, storyid = story.StoryId });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {

                var errorViewModel = new ErrorViewModel
                {

                    RequestId = ex.ToString()
                };
                // Redirect to an error page or show a generic error message
                return RedirectToAction("Error", "Home");
            }
        }
        #region ShareStory saveto database
        [HttpPost]


        //public IActionResult Share_Story(string[] Image, int MissionId, string Title, DateTime Date, string Description, int UserId,string[] videoUrls)
        //{
        //    var alreadyshare = _objUserInterface.alreadystory(MissionId, UserId);
        //    if (alreadyshare)
        //    {
        //        long storyId=_objUserInterface.story(Image, MissionId, Title, Date, Description, UserId,videoUrls);
        //        return Json(new { success = true , storyid = storyId });
        //    }
        //    else
        //    {
        //        return Json(new { success = false });
        //    }
        //}

        #endregion
        [HttpPost]


        public IActionResult Share_StorySubmit(string[] Image, int MissionId, string Title, DateTime Date, string Description, int UserId, string[] videoUrls)
        {
            
            
                long storyId = _objUserInterface.storySubmit(Image, MissionId, Title, Date, Description, UserId, videoUrls);
                return Json(new { success = true, storyid = storyId });
            
            
        }


        #region addtoFav
        [HttpPost]
        public  IActionResult AddFav( long missionId)
        {
            var id = HttpContext.Session.GetString("UserId");
            if(id != null) {
            int u_id = int.Parse(id);
            FavouriteMission favoriteMission = _objUserInterface.FavouriteMission(u_id, missionId);
            bool success = _objUserInterface.Update_favourite(favoriteMission, missionId, u_id);
                return Json(new { success = true, isLiked = success });
            }
            else
            {
                ViewBag.rating = "Login First";
                return View();
            }

        }
        #endregion


        #region AddRatings
        [HttpPost]
        public IActionResult AddRating(string rating ,long missionId)
        {
            var id = HttpContext.Session.GetString("UserId");
           
            if (id != null)
            {
                int u_id = int.Parse(id);
               
               

                MissionRating ratingExists = _objUserInterface.Rating(u_id, missionId);
                MissionApplication Applied = _db.MissionApplications.FirstOrDefault(MA => MA.UserId == u_id && MA.MissionId == missionId && MA.ApprovalStatus == "ACCEPT"); _db.MissionApplications.FirstOrDefault(MA => MA.UserId == u_id && MA.MissionId == missionId && MA.ApprovalStatus == "ACCEPT"); _db.MissionApplications.FirstOrDefault(MA => MA.UserId == u_id && MA.MissionId == missionId && MA.ApprovalStatus == "ACCEPT");
                if (Applied != null)
                {
                    if (ratingExists != null)
                    {
                        _objUserInterface.Update_Rating(ratingExists, rating, u_id, missionId);
                        var ratings = _db.MissionRatings.Where(t => t.MissionId == missionId).ToList();
                        var count = ratings.Count;

                        float ratingdisplay = 0;
                        float sum = 0;
                        foreach (var entry in ratings)
                        {
                            sum = sum + int.Parse(entry.Rating);

                        }
                        ratingdisplay = sum / ratings.Count;
                        return Json(new { success = true, ratingExists, ratingDisplay = ratingdisplay, isRated = true });
                    }
                    else
                    {
                        MissionRating newRating = _objUserInterface.ADD_Rating(ratingExists, rating, u_id, missionId);
                        var ratings = _db.MissionRatings.Where(t => t.MissionId == missionId).ToList();
                        var count = ratings.Count;

                        float ratingdisplay = 0;
                        float sum = 0;
                        foreach (var entry in ratings)
                        {
                            sum = sum + int.Parse(entry.Rating);

                        }
                        ratingdisplay = sum / ratings.Count;
                        return Json(new { success = true, newRating, ratingDisplay = ratingdisplay, isRated = false });
                    }
                }
                    
                return View();
                
            }
            else
            {
                return View();
            }
            

        }
        #endregion

        #region Addcomment 
        [HttpPost]
        public IActionResult Addcomment(string coment,int missionId)
        {
            var id = HttpContext.Session.GetString("UserId");
           
           
            if (id != null && coment!=null)
            {
                int u_id = int.Parse(id);

                MissionApplication Applied = _db.MissionApplications.FirstOrDefault(MA => MA.UserId == u_id && MA.MissionId==missionId && MA.ApprovalStatus=="ACCEPT");
                if (Applied!=null) {
                    _objUserInterface.Add_Comment(coment, missionId, u_id);
                    List<CommentViewModel> commentList = _objUserInterface.getcomment(missionId);
                    return PartialView("_commentPartial", commentList);
                }
                
                return View();

            }
            else
            {
                ViewBag.rating = "Login First";
                return View();
            }


        }

        #endregion

        #region NomissionFound
        public IActionResult NoMissionFound()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (userId != null)
            {
                int u_id = int.Parse(userId);
                LandingPAgeViewModel datauser = _objUserInterface.GetCustomers(1, u_id);
                List<MissionViewModel> missionlistuser = _objUserInterface.getmissions(u_id);
                List<CountryViewModel> countryuser = _objUserInterface.CountryList();
                List<CityViewModel> cityuser = _objUserInterface.CityList();
                List<ThemeViewModel> themeuser = _objUserInterface.ThemeList();
                string jsonCityuser = JsonSerializer.Serialize(cityuser);
                string jsonContryuser = JsonSerializer.Serialize(countryuser);
                string jsonDatauser = JsonSerializer.Serialize(missionlistuser);
                string jsonThemeuser = JsonSerializer.Serialize(themeuser);
                ViewBag.Country = jsonContryuser;
                ViewBag.City = jsonCityuser;
                ViewBag.Theme = jsonThemeuser;
                ViewBag.JsonData = jsonDatauser;
                int countlist = missionlistuser.Count();
                ViewBag.Count = countlist;
                return View(datauser);

            }
            else
            {
                LandingPAgeViewModel data = _objUserInterface.GetCustomers(1, 10003);
                List<MissionViewModel> missionlist = _objUserInterface.getmissions();
                List<CountryViewModel> country = _objUserInterface.CountryList();
                List<CityViewModel> city = _objUserInterface.CityList();
                List<ThemeViewModel> theme = _objUserInterface.ThemeList();
                string jsonCity = JsonSerializer.Serialize(city);
                string jsonContry = JsonSerializer.Serialize(country);
                string jsonData = JsonSerializer.Serialize(missionlist);
                string jsonTheme = JsonSerializer.Serialize(theme);
                ViewBag.Country = jsonContry;
                ViewBag.City = jsonCity;
                ViewBag.Theme = jsonTheme;
                ViewBag.JsonData = jsonData;
                int count = missionlist.Count();
                ViewBag.Count = count;
                return View(data);

            }
        }
        #endregion


        [HttpPost]
        public IActionResult Contactus(string name, string mail, string subject, string message)
        {
            var userId = HttpContext.Session.GetString("UserId");
            _objUserInterface.contactadd(name, mail, subject, message, int.Parse(userId));
            return Json(new { success = true });
        }
        public MissionCardModel CreateCard(Mission mission)
        {
            var userId = HttpContext.Session.GetString("UserId");
            int u_id = int.Parse(userId);
            var card = new MissionCardModel();
            card.mission = mission;
            var img = _db.MissionMedia.FirstOrDefault(x => x.MissionId == mission.MissionId);
            card.CardImg = img != null ? img.MediaPath : "~/images/Grow-Trees-On-the-path-to-environment-sustainability-1.png";
            card.seatsLeft = (int)(int.Parse(mission.SeatAvailable) - (_db.MissionApplications.Where(x => x.MissionId == mission.MissionId).Count()));
            card.goalMission = _db.GoalMissions.FirstOrDefault(x => x.MissionId == mission.MissionId);
            if (card.goalMission != null)
            {
                float totalGoal = card.goalMission.GoalValue;
                card.progressBar = totalGoal;
            }
            card.missionApplied = _db.MissionApplications.FirstOrDefault(x => x.MissionId == mission.MissionId && x.UserId == u_id) != null ? 1 : 0;
            card.approvalPending = _db.MissionApplications.FirstOrDefault(x => x.MissionId == mission.MissionId && x.UserId == u_id
&& x.ApprovalStatus == "PENDING") != null ? 1 : 0;
            var id = HttpContext.Session.GetString("UserId");
            if (id != null)
            {
                int uid = int.Parse(id);
                card.favMission = _db.FavouriteMissions.FirstOrDefault(x => x.MissionId == mission.MissionId && x.UserId == uid && x.DeletedAt == null) != null ? 1 : 0;
            }
            var ratings = _db.MissionRatings.Where(t => t.MissionId == mission.MissionId).ToList();
            var rating = 0;
            var sum = 0;
            foreach (var entry in ratings)
            {
                sum = sum + int.Parse(entry.Rating);

            }
            rating = sum / ratings.Count;
            if (rating != null)
            {
                card.missionRating = rating;
            }

            card.theme = _db.MissionThemes.FirstOrDefault(x => x.MissionThemeId == mission.ThemeId).Title;
            card.country = _db.Cities.FirstOrDefault(x => x.CityId == mission.CityId).Name;


            return card;
        }

        #region StoryListing
        [Authorize]
        public IActionResult storyListingPage(int pg = 1)
        {
            List<storyListingViewModel> list = new List<storyListingViewModel>();
            List<Story>stories= _db.Stories.Where(st=>st.Status=="DRAFT"&& st.DeletedAt==null).ToList();
            foreach (var data in stories)
            {
                storyListingViewModel listView = new storyListingViewModel();
                listView.Title = data.Title;
                listView.Description = data.Description;
                
                var mission=_db.Missions.Where(x => x.MissionId==data.MissionId).FirstOrDefault();
                var themeName=_db.MissionThemes.Where(x => x.MissionThemeId==mission.ThemeId).FirstOrDefault();
                var imageNull = _db.MissionMedia.Where(x => x.MissionId == data.MissionId).FirstOrDefault();
                var image= _db.StoryMedia.Where(x => x.StoryId == data.StoryId).FirstOrDefault();
                if (image!=null)
                {
                    listView.image = image.Path;
                }
                else
                {
                    listView.image = imageNull.MediaPath;
                }
               
                listView.Theme = themeName.Title;
                listView.StoryId = data.StoryId;
                listView.MissionId=data.MissionId;
                var user=_db.Users.Where(x => x.UserId==data.UserId).FirstOrDefault();
                listView.UserName = user.FirstName;
                listView.Avtar = user.Avatar == null ? "/images/user1.png" : user.Avatar;
                list.Add(listView);
            }
            ViewBag.list= list;
            ViewBag.slugs = _db.CmsPages.Where(m => m.DeletedAt == null).ToList();
            const int pageSize = 6;
            int recsCount = list.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
           
            var datas = list.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.list = datas;
            this.ViewBag.Pager = pager;
            this.ViewBag.count = recsCount;
          

            return View();
        
        }

        #endregion

        #region recentVolunteer

        public ActionResult GetRecentVolunteers(long id,int page = 1)
        {
            int pageSize = 2; // Number of items per page
            var temp = _db.MissionApplications.Where(x => x.MissionId == id).ToList();
            var listVolunteering = new List<RecentVolunteerVM>();
            foreach (var u in temp)
            {
                var volunteeringModel = new RecentVolunteerVM();
                var user1 = _db.Users.FirstOrDefault(x => x.UserId == u.UserId);
                volunteeringModel.missions = user1.FirstName + " " + user1.LastName;
                volunteeringModel.users = user1.Avatar != null ? user1.Avatar : "/images/user1.png";
                listVolunteering.Add(volunteeringModel);
            } // Get the items from the database

            int totalItems = listVolunteering.Count(); // Get the total number of items
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Calculate the total number of pages

            var pagedItems = listVolunteering.Skip((page - 1) * pageSize).Take(pageSize).ToList(); ; // Get the items for the current page

            var model = new RecentVolPagination
            {
                Items = pagedItems,
                CurrentPage = page,
                TotalPages = totalPages // Add the total number of pages to the model
            };


            return View(model); // Return the partial view with the paged items
        }
        #endregion


        #region storydDetail
        [Authorize]
        public IActionResult StoryDetailPage(int storyid, int? usersid)
        {
            if (usersid==null)
            {
                var id = HttpContext.Session.GetString("UserId");

                int u_id = int.Parse(id);
                storyListingViewModel list = new storyListingViewModel();
                Story stories = _db.Stories.Where(st => st.StoryId == storyid).FirstOrDefault();
                List<User> userlist = _objILogin.Users(u_id);
                storyListingViewModel listView = new storyListingViewModel();
                listView.Title = stories.Title;
                listView.StoryId = stories.StoryId;
                listView.Description = stories.Description;
                var mission = _db.Missions.Where(x => x.MissionId == stories.MissionId).FirstOrDefault();
                var themeName = _db.MissionThemes.Where(x => x.MissionThemeId == mission.ThemeId).FirstOrDefault();
                listView.Theme = themeName.Title;
                var images = _db.StoryMedia.Where(t => t.StoryId == storyid && t.Type == "image").ToList();
                var media = _db.StoryMedia.Where(t => t.StoryId == storyid).ToList();
                if (images != null)
                {
                    var mediaPaths = new List<string>();
                    foreach (var image1 in images)
                    {
                        mediaPaths.Add(image1.Path);
                    }
                    listView.MediaPaths = mediaPaths;
                }
                listView.Media = media;

                listView.Views = (long)(stories.Views + 1);
                listView.MissionId = stories.MissionId;
                var user = _db.Users.Where(x => x.UserId == stories.UserId).FirstOrDefault();
                listView.why_i_volunteer = user.WhyIVolunteer != null ? user.WhyIVolunteer : "Lorem ipsum dolor sit amet . The graphic and typographic operators know this well, in reality all the professions dealing with the universe of communication have a stable relationship with these words, but what is it ? Lorem ipsum is a dummy text without any sense.It is a sequence of Latin words that, as they are positioned, do not form sentences with a complete sense, but give life to a test text useful to fill spaces that will subsequently be occupied from ad hoc texts composed by communication professionals.It is certainly the most famous placeholder text even if there are different versions distinguishable from the order in which the Latin words are repeated.Lorem ipsum contains the typefaces more in use, an aspect that allows you to have an overview of the rendering of the text in terms of font choice and font siz";
                listView.UserName = user.FirstName;
                listView.Avtar = user.Avatar == null ? "/images/user1.png" : user.Avatar;
                ViewBag.Users = userlist;
                stories.Views = listView.Views;
                ViewBag.slugs = _db.CmsPages.Where(m => m.DeletedAt == null).ToList();
                _db.Stories.Update(stories);
                _db.SaveChanges();

                return View(listView);
            }
            else
            {
                int u_id = (int)usersid;
                storyListingViewModel list = new storyListingViewModel();
                Story stories = _db.Stories.Where(st => st.StoryId == storyid).FirstOrDefault();
                List<User> userlist = _objILogin.Users(u_id);
                storyListingViewModel listView = new storyListingViewModel();
                listView.Title = stories.Title;
                listView.Description = stories.Description;
                var mission = _db.Missions.Where(x => x.MissionId == stories.MissionId).FirstOrDefault();
                var themeName = _db.MissionThemes.Where(x => x.MissionThemeId == mission.ThemeId).FirstOrDefault();
                listView.Theme = themeName.Title;
                var images = _db.StoryMedia.Where(t => t.StoryId == storyid && t.Type == "image").ToList();
                var media = _db.StoryMedia.Where(t => t.StoryId == storyid).ToList();
                if (images != null)
                {
                    var mediaPaths = new List<string>();
                    foreach (var image1 in images)
                    {
                        mediaPaths.Add(image1.Path);
                    }
                    listView.MediaPaths = mediaPaths;
                }
                listView.Media = media;
                listView.Views = (long)(stories.Views + 1);
                listView.MissionId = stories.MissionId;
                var user = _db.Users.Where(x => x.UserId == stories.UserId).FirstOrDefault();
                listView.why_i_volunteer = user.WhyIVolunteer != null ? user.WhyIVolunteer : "Lorem ipsum dolor sit amet . The graphic and typographic operators know this well, in reality all the professions dealing with the universe of communication have a stable relationship with these words, but what is it ? Lorem ipsum is a dummy text without any sense.It is a sequence of Latin words that, as they are positioned, do not form sentences with a complete sense, but give life to a test text useful to fill spaces that will subsequently be occupied from ad hoc texts composed by communication professionals.It is certainly the most famous placeholder text even if there are different versions distinguishable from the order in which the Latin words are repeated.Lorem ipsum contains the typefaces more in use, an aspect that allows you to have an overview of the rendering of the text in terms of font choice and font siz";
                listView.UserName = user.FirstName;
                listView.Avtar = user.Avatar == null ? "/images/user1.png" : user.Avatar;
                ViewBag.Users = userlist;
                ViewBag.slugs = _db.CmsPages.Where(m => m.DeletedAt == null).ToList();
                stories.Views = listView.Views;
                _db.Stories.Update(stories);
                _db.SaveChanges();

                return View(listView);
            }
            
        }

        #endregion

    
        #region Share Story

        [Authorize]
        public IActionResult ShareYourStory()
        {
            var U_Id = HttpContext.Session.GetString("UserId");
           ShareStoryViewModel sharestory = _objUserInterface.getsharestory(int.Parse(U_Id));
            ViewBag.slugs = _db.CmsPages.Where(m => m.DeletedAt == null).ToList();
            return View(sharestory);
        }

        #endregion

        #region PostShareStory
        [HttpPost]
        public IActionResult ShareYourStory(int i)
        {

            return View();
        }
        #endregion

        #region storyPreview
        [HttpPost]
        public JsonResult StoryPreview(long missionId)
        {
            //if (story != null && story.Status == "DRAFT")
            //{
            //    List<StoryMedium> media = _objStoryListing.searchmedia(story.StoryId);
            //    var mediaObjects = media.Select(m => new { Path = m.Path }).ToArray();
            //    return Json(new { success = true, story = story, storyimage = mediaObjects });
            //}
            //else if (story != null && story.Status == "pending")
            //{
            //    return Json(new { success = false });
            //}
            //else
            //{
            //    return Json(new { success = "notadded" });
            //}

            var userId=HttpContext.Session.GetString("UserId");
            Story storypreview = _db.Stories.FirstOrDefault(st => st.UserId == int.Parse(userId) && st.MissionId == missionId);
            if (storypreview==null)
            {
                return Json(new { success = "notadded" }); 
            }
            else if (storypreview.Status == "DRAFT" && storypreview != null)
            {
                List<StoryMedium> media = _db.StoryMedia.Where(sm => sm.StoryId == storypreview.StoryId && sm.Type == "image").ToList();
                List<string> pathList = media.Select(m => m.Path).ToList();
                List<StoryMedium> mediaVideo = _db.StoryMedia.Where(sm => sm.StoryId == storypreview.StoryId && sm.Type == "Video").ToList();
                List<string> VideoPath = mediaVideo.Select(m => m.Path).ToList();
                var data = new { success = true, storypreview, pathList, VideoPath };
                return Json(data);
            }
            else if (storypreview.Status=="PENDING" && storypreview.DeletedAt!=null)
            {
                return Json(new { success = "Deleted" });
            }
            else
            {
                return Json(new { success = false });

            }
        }
        #endregion

        [Authorize]
        #region user edit
        public IActionResult userEditProfile(long? CountryId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            List<City> city;
            if (CountryId==null) {
               city = _objUserInterface.cities();
            }
            else
            {
                city = _objUserInterface.cities((long)CountryId);
            }
            List<Country> country = _objUserInterface.countries();
            List<Skill> skill = _objUserInterface.skills(int.Parse(userId));
            List<SelectListItem> listCities = new List<SelectListItem>();
            List<SelectListItem> listCountries = new List<SelectListItem>();
            List<SelectListItem> listSkills = new List<SelectListItem>();
            List<SelectListItem> oneuserskill = new List<SelectListItem>();
            User user = _objILogin.GetUsers(int.Parse(userId));
            Userviewmodel userviewmodel = new Userviewmodel();
            var userskill = _objUserInterface.oneuserskill(int.Parse(userId));
            foreach (var item in city)
            {
                listCities.Add(new SelectListItem() { Text = item.Name, Value = item.CityId.ToString() });
            }
            foreach (var item in country)
            {
                listCountries.Add(new SelectListItem() { Text = item.Name, Value = item.CountryId.ToString() });
            }
            foreach (var item in skill)
            {
                listSkills.Add(new SelectListItem() { Text = item.SkillName, Value = item.SkillId.ToString() });
            }
            foreach (var item in userskill)
            {
                oneuserskill.Add(new SelectListItem() { Text = item.SkillName, Value = item.SkillId.ToString() });
            }
            userviewmodel.cities = listCities;
            userviewmodel.countries = listCountries;
            userviewmodel.skills = listSkills;
            userviewmodel.userskill = oneuserskill;
            userviewmodel.UserId = user.UserId;
            userviewmodel.FirstName = user.FirstName;
            userviewmodel.LastName = user.LastName;
            userviewmodel.Availability = user.Availability; 
            userviewmodel.Avatar = user.Avatar;
            userviewmodel.WhyIVolunteer = user.WhyIVolunteer;
            userviewmodel.EmployeeId = user.EmployeeId;
            userviewmodel.Status = user.Status;
            userviewmodel.Department = user.Department;
            userviewmodel.CityId = user.CityId;
            userviewmodel.CountryId = user.CountryId;
            userviewmodel.ManagerDetail = user.ManagerDetail;
            userviewmodel.ProfileText = user.ProfileText;
            userviewmodel.Title = user.Title;
            userviewmodel.LinkedInUrl = user.LinkedInUrl;
            ViewBag.slugs = _db.CmsPages.Where(m => m.DeletedAt == null).ToList();
            return View(userviewmodel);
        }
        #endregion

        #region userEditPost
        [HttpPost]
        public IActionResult UserEdit(Userviewmodel userViewModel, string[] skill)
        {
            var userId = HttpContext.Session.GetString("UserId");
            HttpContext.Session.SetString("Avtar", userViewModel.Avatar == null ? "/images/user1.png" : userViewModel.Avatar);
            HttpContext.Session.SetString("UserName", userViewModel.FirstName == null ? "User" : userViewModel.FirstName);
            if (skill.Length > 0)
            {
                _objUserInterface.saveskill(skill, int.Parse(userId));
            }
            
            _objUserInterface.adduser(userViewModel, int.Parse(userId));
            return RedirectToAction("userEditProfile", "Home");
        }
        #endregion

        #region password Edit
        [HttpPost]
        public IActionResult passEdit(string old, string newp, string confp)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var savepass = _objUserInterface.savePassword(old, newp, confp, int.Parse(userId));
            if (savepass == true)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        #endregion


        public IActionResult VolunteeerTimesheet()
        {
            var userId = HttpContext.Session.GetString("UserId");
            List<Mission> missiontime = _objUserInterface.missionstime(int.Parse(userId));
            List<Mission> missiongoal = _objUserInterface.missionsgoal(int.Parse(userId));
            List<SelectListItem> listmissiontime = new List<SelectListItem>();
            List<SelectListItem> listmissiongoal = new List<SelectListItem>();
            List<TimesheetViewModel> sheetview = new List<TimesheetViewModel>();
            List<TimesheetViewModel> sheetview2 = new List<TimesheetViewModel>();

            List<Timesheet> sheetviewtime = _objUserInterface.timesheetlistTime(int.Parse(userId));
            var sheetrecordtime = (from sv in sheetviewtime join mt in missiontime on sv.MissionId equals mt.MissionId select new { sheetid = sv.TimesheetId, Name = mt.Title, Timespend = sv.Time, Date = sv.DateVolunteered }).ToList();

            foreach (var item in sheetrecordtime)
            {
                TimesheetViewModel timesheetViewModel2 = new TimesheetViewModel();
                timesheetViewModel2.TimesheetId = item.sheetid;
                timesheetViewModel2.Title = item.Name;
                timesheetViewModel2.Timehour = item.Timespend.Split(':').First();
                timesheetViewModel2.Timeminute = item.Timespend.Split(':').Last();
                timesheetViewModel2.DateVolunteered = item.Date;
                sheetview2.Add(timesheetViewModel2);
            }


           
            List<Timesheet> timesheets = _objUserInterface.timesheetlist(int.Parse(userId));
            var sheetrecord = (from ts in timesheets join mg in missiongoal on ts.MissionId equals mg.MissionId select new { sheetid = ts.TimesheetId, Name = mg.Title, Action = ts.Action, Date = ts.DateVolunteered }).ToList();
            TimesheetViewModel timesheetViewModel = new TimesheetViewModel();
            foreach (var item in missiontime)
            {
                listmissiontime.Add(new SelectListItem { Text = item.Title, Value = item.MissionId.ToString() });
            }
            foreach (var item in missiongoal)
            {
                listmissiongoal.Add(new SelectListItem { Text = item.Title, Value = item.MissionId.ToString() });
            }
            foreach (var item in sheetrecord)
            {
                TimesheetViewModel timesheetViewModel1 = new TimesheetViewModel();
                timesheetViewModel1.TimesheetId = item.sheetid;
                timesheetViewModel1.Title = item.Name;
                timesheetViewModel1.Action = item.Action.ToString();
                timesheetViewModel1.DateVolunteered = item.Date;
                sheetview.Add(timesheetViewModel1);
            }
            timesheetViewModel.timesheets = sheetview;
            timesheetViewModel.timesheettime = sheetview2;
            timesheetViewModel.missionstime = listmissiontime;
            timesheetViewModel.missionsgoal = listmissiongoal;
            ViewBag.slugs = _db.CmsPages.Where(m => m.DeletedAt == null).ToList();
            return View(timesheetViewModel);
            
        }

        [HttpPost]
        public IActionResult CheckAction(string inputValue, long missionId)
        {
            var goalvalue = _db.GoalMissions.Where(g => g.MissionId == missionId).FirstOrDefault();
            if (goalvalue!=null)
            {
                if (goalvalue.GoalValue<int.Parse(inputValue))
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public IActionResult CheckDate(long missionid, DateTime volundate)
        {
            var findmissiondate = _db.Missions.Where(m => m.MissionId == missionid).FirstOrDefault();
            if (findmissiondate == null)
            {
                return Json(new { message = "Mission not found." });
            }

            DateTime? sdate = findmissiondate.StartDate ?? DateTime.MinValue;
            DateTime startDate = sdate.HasValue ? sdate.Value.Date : DateTime.MinValue;
            DateTime? edate = findmissiondate.EndDate ?? DateTime.MinValue;
            DateTime endDate = edate.HasValue ? edate.Value.Date : DateTime.MinValue;
            if (volundate < startDate || volundate > endDate)
            {
                return Json(new { message = "Please enter a date between " + startDate.ToString("yyyy-MM-dd") + " and " + endDate.ToString("yyyy-MM-dd") });
            }
            else
            {
                return Json(new { success = true });
            }
        }
        [HttpPost]
        public IActionResult editTime(int timesheetid)
        {
            var find = _objUserInterface.findtimerecord(timesheetid);
            return Json(new { find = find });
        }
        [HttpPost]
        public IActionResult TimesheetTime(TimesheetViewModel timesheetviewmodel)
        {
            var userId = HttpContext.Session.GetString("UserId");
            _objUserInterface.sheetime(timesheetviewmodel, int.Parse(userId));
            return RedirectToAction("VolunteeerTimesheet", "Home");
        }
        [HttpPost]
        public IActionResult VolunteeerTimesheet(TimesheetViewModel timesheetviewmodel)
        {
            var userId = HttpContext.Session.GetString("UserId");
            _objUserInterface.timesheet(timesheetviewmodel, int.Parse(userId));
            List<Mission> missiontime = _objUserInterface.missionstime(int.Parse(userId));
            List<Mission> missiongoal = _objUserInterface.missionsgoal(int.Parse(userId));
            List<SelectListItem> listmissiontime = new List<SelectListItem>();
            List<SelectListItem> listmissiongoal = new List<SelectListItem>();
            List<TimesheetViewModel> sheetview = new List<TimesheetViewModel>();
            List<Timesheet> timesheets = _objUserInterface.timesheetlist(int.Parse(userId));
            var sheetrecord = (from ts in timesheets join mg in missiongoal on ts.MissionId equals mg.MissionId select new { sheetid = ts.TimesheetId, Name = mg.Title, Action = ts.Action, Date = ts.DateVolunteered }).ToList();
            TimesheetViewModel timesheetViewModel = new TimesheetViewModel();
            foreach (var item in missiontime)
            {
                listmissiontime.Add(new SelectListItem { Text = item.Title, Value = item.MissionId.ToString() });
            }
            foreach (var item in missiongoal)
            {
                listmissiongoal.Add(new SelectListItem { Text = item.Title, Value = item.MissionId.ToString() });
            }
            foreach (var item in sheetrecord)
            {
                TimesheetViewModel timesheetViewModel1 = new TimesheetViewModel();
                timesheetViewModel1.TimesheetId = item.sheetid;
                timesheetViewModel1.Title = item.Name;
                timesheetViewModel1.Action = item.Action.ToString();
                timesheetViewModel1.DateVolunteered = item.Date;
                sheetview.Add(timesheetViewModel1);
            }
            timesheetviewmodel.timesheets = sheetview;
            timesheetviewmodel.missionstime = listmissiontime;
            timesheetviewmodel.missionsgoal = listmissiongoal;
            return RedirectToAction("VolunteeerTimesheet", "Home");

        }

        [HttpPost]
        public IActionResult edit(long missionid)
        {
            Timesheet timesheetviewmodel = _db.Timesheets.Where(tm=>tm.TimesheetId==missionid).FirstOrDefault();
            return Json(new { timesheet = timesheetviewmodel });

        }
        [HttpPost]
        public IActionResult delete_post(long missionId)
        {
            Timesheet timesheetviewmodel = _db.Timesheets.Where(tm => tm.TimesheetId == missionId).FirstOrDefault();
            _db.Timesheets.Remove(timesheetviewmodel);
            _db.SaveChanges();
            return Json(new { success = true });

        }

        [HttpPost]
        public IActionResult changePassword(Userviewmodel userViewModel)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var savepass = _objUserInterface.savePassword(userViewModel.Password, userViewModel.NewPassword, userViewModel.confirmPasswrd, int.Parse(userId));
            if (savepass == true)
            {
                return RedirectToAction("userEditProfile","home");
            }
            else
            {
                return RedirectToAction("userEditProfile", "home"); ;
            }

        }
        public IActionResult dummy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.slugs = _db.CmsPages.Where(m => m.DeletedAt == null).ToList();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}