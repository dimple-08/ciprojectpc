using CI_Platform.Entity.CIDbContext;
using CI_Platform.Entity.DataModels;
using CI_Platform.Entity.ViewModels;
using CI_Platform.Repository.Interfaces;
using CI_Platform.Repository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _IUserRepositoty;
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Register 
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            var userRegister = _userRepository.RegisterUser(user);
            if (userRegister)
            {
                return RedirectToAction("Login");
            }
            else
            {
                @ViewBag.EmailExists = "Email Already Exists";
                return View();
            }
        }
        #endregion

        #region Login

        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            var loginsucc = _userRepository.LoginUser(login);
            if (loginsucc.UserId != 0)
            {
                HttpContext.Session.SetString("UserId", loginsucc.UserId.ToString());
                HttpContext.Session.SetString("UserName", loginsucc.FirstName);
                return RedirectToAction("LandingPage", "LandingPage");

            }
            else
            {
                //ModelState.AddModelError("LoginFailed", "Email or Password is incorrect");
                ViewBag.LoginError = "Email or Password is incorrect or Empty";
                return View();
            }
        }

        #endregion

        #region Forgot

        public IActionResult ForgotPass()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPass(ForgotModel forgot)
        {
            bool forgotsucc = _userRepository.ForgotPassword(forgot);
            if (forgotsucc)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.emailnotexist = "Email not Exists Please Register First";
                return View();
            }
        }
        #endregion

        #region Reset 

        public IActionResult ResetPass(string email, string token)
        {
            var resetPass = _userRepository.ResetPasswordGet(email, token);
            if (resetPass.Email == null && resetPass.Token == null)
            {
                return RedirectToAction("ForgotPass");
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public IActionResult ResetPassPost(ResetModel reset)
        {
            var resetPassUpdate = _userRepository.ResetPasswordPost(reset);
            if (resetPassUpdate)
            {
                ViewBag.ResetSuccessfully = "Password change successfully plese click on login  👇🏻👇🏻👇🏻 ";
                return View("ResetPass");

            }
            else
            {
                return RedirectToAction("ForgotPass");

            }
        }


        #endregion
    }
}