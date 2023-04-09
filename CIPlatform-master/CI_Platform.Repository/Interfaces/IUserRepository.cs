using CI_Platform.Entity.DataModels;
using CI_Platform.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interfaces
{
    public interface IUserRepository
    {
        public bool RegisterUser(User user);

        public User LoginUser(LoginModel login);

        public bool ForgotPassword(ForgotModel forgot);

        public ResetModel ResetPasswordGet(string email, string token);

        public bool ResetPasswordPost(ResetModel reset);
    }
}
