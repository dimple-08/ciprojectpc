using CIProjectweb.Entities.DataModels;
using CIProjectweb.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Repository.Repository.Interface
{
   public interface ILogin
    {
        public User GetUsers(int uID);

        public string getUserName(LoginViewModel objlogin);
        public List<User> Users(int id);
        public int validateUser(LoginViewModel objlogin);
    }
}
