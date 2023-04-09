using CI_Platform.Entity.CIDbContext;
using CI_Platform.Entity.DataModels;
using CI_Platform.Entity.ViewModels;
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
    public class UserRepository : IUserRepository
    {
        private readonly CiPlatformContext _CiPlatformContext;

        public UserRepository(CiPlatformContext CiPlatformContext)
        {
            _CiPlatformContext = CiPlatformContext;
        }


        public bool RegisterUser(User user)
        {
            User data = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Password = user.Password
            };
            if (_CiPlatformContext.Users.FirstOrDefault(u => u.Email == user.Email) == null)
            {
                _CiPlatformContext.Users.Add(data);
                _CiPlatformContext.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public User LoginUser(LoginModel login)
        {
            var user = _CiPlatformContext.Users.FirstOrDefault(x => x.Email == login.Email && x.Password == login.Password);
            if(user != null)
            {
                User model = new User()
                {
                    FirstName = user.FirstName,
                    UserId = user.UserId
                };
                return model;
            }
            else
            {
                User model = new User()
                {
                    FirstName = null
                };
                return model;
            }
            
        }

        public bool ForgotPassword(ForgotModel forgot)
        {
            var user = _CiPlatformContext.Users.FirstOrDefault(u => u.Email == forgot.Email);
            if (user != null)
            {
                var token = Guid.NewGuid().ToString();

                // Store the token in the password resets table with the user's email
                PasswordReset passwordReset = new PasswordReset
                {
                    Email = forgot.Email,
                    Token = token
                };
                _CiPlatformContext.PasswordResets.Add(passwordReset);
                _CiPlatformContext.SaveChanges();

                // Send an email with the password reset link to the user's email address
                UriBuilder builder = new UriBuilder();
                builder.Scheme = "https";
                builder.Host = "localhost";
                builder.Port = 7264; 
                builder.Path = "/User/ResetPass";
                builder.Query = "token=" + token + "&email=" + forgot.Email;
                var resetLink = builder.ToString();
                // Send email to user with reset password link
                // ...
                MailAddress fromAddress = new MailAddress("evanzandu@gmail.com", "CI Platform");
                MailAddress toAddress = new MailAddress(forgot.Email);
                var subject = "Password reset request";
                //var body = $"Hi,<br /><br />Please click on the following link to reset your password:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
                var body = $"Hello,<br /><br /><a href='{resetLink}'>Click here to Reset your Password</a>";
                MailMessage message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("evanzandu@gmail.com", "timrrquqhqzvdpns"),
                    EnableSsl = true
                };
                smtpClient.Send(message);



                return true;
            }
            else
            {
                return false;
            }

        }

        public ResetModel ResetPasswordGet(string email, string token)
        {
            var passwordReset = _CiPlatformContext.PasswordResets.FirstOrDefault(pr => pr.Email == email && pr.Token == token);
            if (passwordReset == null)
            {
                ResetModel model = new ResetModel
                {
                    Email = null,
                    Token = null
                };
                return model;
            }
            else
            {
                ResetModel model = new ResetModel
                {
                    Email = email,
                    Token = token
                };
                return model;
            }
        }


        public bool ResetPasswordPost(ResetModel reset)
        {
            var user = _CiPlatformContext.Users.FirstOrDefault(u => u.Email == reset.Email);
            if (user == null)
            {
                return false;
            }

            // Find the password reset record by email and token
            var passwordReset = _CiPlatformContext.PasswordResets.FirstOrDefault(pr => pr.Email == reset.Email && pr.Token == reset.Token);
            if (passwordReset == null)
            {
                return false;
            }

            // Update the user's password
            user.Password = reset.Password;
            //user.UpdatedAt = rpm.UpdatedAt;
            _CiPlatformContext.SaveChanges();


            return true;

        }

    }
}
