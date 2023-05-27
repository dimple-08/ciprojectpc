using DummyLMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace DummyLMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Login()
        {
          
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM loginModel)
        {
            if (ModelState.IsValid && WebSecurity.UserExists(loginModel.userName))
            {
                
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(loginModel);
            }

           
        }
        public bool ValidateUser(string userName,string Password)
        {
            LMSEntitiesContext lmsentitiesContext = new LMSEntitiesContext();
            var user= lmsentitiesContext.tblLogins.Where(t => t.userName.Equals(userName) && t.Password.Equals(Password)).FirstOrDefault();
            if (user!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
