using CIProjectweb.Entities.DataModels;
using CIProjectweb.Entities.ViewModels;
using CIProjectweb.Repository.Repository.Interface;


namespace CIProjectweb.Repository.Repository
{
    public class Login:ILogin
    {
        private readonly CIDbContext _objdb;

        public Login(CIDbContext objdb)
        {
            _objdb = objdb;
        }
        public User GetUsers(int uid)
        {
            return _objdb.Users.Where(a => a.UserId == uid).SingleOrDefault();
        }
        public string getUserName(LoginViewModel objlogin)
        {
            var user = _objdb.Users.Where(a => a.Email.Equals(objlogin.Email)).FirstOrDefault();
            if (user != null)
            {

                var obj = _objdb.Users.Where(a => a.Email.Equals(objlogin.Email) && a.Password.Equals(objlogin.Password)).FirstOrDefault();
                if (obj != null)
                {
                    return obj.FirstName;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
        public int validateUser(LoginViewModel objlogin)
        {
            var user = _objdb.Users.Where(a => a.Email.Equals(objlogin.Email) ).FirstOrDefault();
            if (user != null)
            {

                var obj = _objdb.Users.Where(a => a.Email.Equals(objlogin.Email) && a.Password.Equals(objlogin.Password) && a.Status == true && a.DeletedAt == null).FirstOrDefault();
                if (obj != null)
                {
                    return (int)obj.UserId;
                }
                else
                {
                    return 0;
                }
                
            }
            else
            {
                return 0;
            }
        }


        public List<User> Users(int id)
        {
            return _objdb.Users.Where(u=>u.UserId!=id && u.DeletedAt==null && u.Status==true).ToList();
        }
    }
}
