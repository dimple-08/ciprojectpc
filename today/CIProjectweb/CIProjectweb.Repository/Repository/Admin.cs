using CIProjectweb.Entities.AdminViewModel;
using CIProjectweb.Entities.DataModels;
using CIProjectweb.Repository.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Repository.Repository
{
    public class Admin:IAdmin
    {
        private readonly CIDbContext _objdb;
        public Admin(CIDbContext objdb)
        {
           
                _objdb = objdb;
        }

        public List<User> alluser()
        {
            List<User> users = _objdb.Users.Where(u=>u.DeletedAt==null).ToList();
            return users;
        }

        public List<City> cities()
        {
            List<City> city = _objdb.Cities.ToList();
            return city;
        }

        public List<Country> countries()
        {
            List<Country> country = _objdb.Countries.ToList();
            return country;
        }
        public bool AddUser(string FirstName, string Lastname, string Email, string EmployeeId, string Password, string Department, long City, long Country, string Profile, string Status,string Avtar,long UserId)
        {
            if (UserId == 0)
            {
                User userAdd = new User();
                userAdd.FirstName = FirstName;
                userAdd.LastName = Lastname;
                userAdd.Password = Password;
                userAdd.Email = Email;
                userAdd.CityId = City;
                userAdd.CountryId = Country;
                if (Status == "Active")
                {
                    userAdd.Status = true;
                }
                else
                {
                    userAdd.Status = false;

                }
                if (Avtar == null)
                {
                    userAdd.Avatar = "/images/user1.png";
                }
                else
                {
                    userAdd.Avatar = Avtar;
                }
                userAdd.EmployeeId = EmployeeId;
                userAdd.Department = Department;
                userAdd.ProfileText = Profile;
                _objdb.Add(userAdd);
                _objdb.SaveChanges();

                return true;
            }
            else
            {
                User user=_objdb.Users.Where(x => x.UserId== UserId).FirstOrDefault();
               
                user.FirstName = FirstName;
                user.LastName = Lastname;
                user.Password = Password;
                user.Email = Email;
                user.CityId = City;
                user.CountryId = Country;
                if (Status == "Active")
                {
                    user.Status = true;
                }
                else
                {
                    user.Status = false;

                }
                if (Avtar==null)
                {
                    user.Avatar = "/images/user1.png";
                }
                else
                {
                    user.Avatar = Avtar;
                }
                
                user.EmployeeId = EmployeeId;
                user.Department = Department;
                user.ProfileText = Profile;
                _objdb.Update(user);
                _objdb.SaveChanges();
                return true;
            }
           
        }
        public void deleteuser(long userId)
        {
            var user = _objdb.Users.FirstOrDefault(u => u.UserId == userId);
            user.DeletedAt = DateTime.Now;
            _objdb.Users.Update(user);
            _objdb.SaveChanges();
        }
        public void deleteCMS(long PageId)
        {
            var user = _objdb.CmsPages.FirstOrDefault(u => u.CmsPageId == PageId);
            user.DeletedAt = DateTime.Now;
            _objdb.CmsPages.Update(user);
            _objdb.SaveChanges();
        }
        public List<Story> allstory()
        {
            List<Story> stories = _objdb.Stories.ToList();
            return stories;
        }
        public bool ADDCms(CMSViewModel cmsadd)
        {
            //CmsPage cmsExists = _objdb.CmsPages.Where(cms => cms.Title == cmsadd.Title).FirstOrDefault();
            //if (cmsExists == null)
            //{
            //    CmsPage cms=new CmsPage();
            //    cms.Title = cmsadd.Title;
            //    cms.Description = cmsadd.Description;
                
            //    cms.Status= cmsadd.Status;
            //    cms.Slug = cmsadd.Slug;
            //    _objdb.CmsPages.Add(cms);
            //    _objdb.SaveChanges();
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            if (cmsadd.CmsPageId==0)
            {
                CmsPage cms = new CmsPage();
                cms.Title = cmsadd.Title;
                cms.Description = cmsadd.Description;

                cms.Status = cmsadd.Status;
                cms.Slug = cmsadd.Slug;
                _objdb.CmsPages.Add(cms);
                _objdb.SaveChanges();
                return true;
            }
            else
            {
                CmsPage cmsExists = _objdb.CmsPages.Where(cms => cms.CmsPageId == cmsadd.CmsPageId).FirstOrDefault();
                cmsExists.Title = cmsadd.Title;
                cmsExists.Description = cmsadd.Description;

                cmsExists.Status = cmsadd.Status;
                cmsExists.Slug = cmsadd.Slug;
                _objdb.CmsPages.Update(cmsExists);
                _objdb.SaveChanges();
                return false;
            }
        }
        public CmsPage EditCMS(long CMSPAgeID)
        {
            return _objdb.CmsPages.Where(u => u.CmsPageId == CMSPAgeID).FirstOrDefault();
        }
        public List<MissionApplication> allmissionapp()
        {
            List<MissionApplication> missionApplications = _objdb.MissionApplications.Where(ma=>ma.ApprovalStatus== "PENDING").ToList();
            return missionApplications;
        }
        public CMSViewModel cmsrecordall()
        {
            CMSViewModel cms = new CMSViewModel();
            cms.cMSViewModels= _objdb.CmsPages.Where(cms=>cms.DeletedAt==null).ToList();
            return cms;
        }
        public List<Mission> allmission()
        {
            List<Mission> missions = _objdb.Missions.ToList();
            return missions;
        }
        public bool Approve(long applicationId)
        {
            MissionApplication mission=_objdb.MissionApplications.Where(ma=>ma.MissionApplicationId==applicationId).FirstOrDefault();
            mission.ApprovalStatus = "ACCEPT";
            _objdb.SaveChanges();
            return true;
        }
        public bool Decline(long applicationId)
        {
            MissionApplication mission = _objdb.MissionApplications.Where(ma => ma.MissionApplicationId == applicationId).FirstOrDefault();
            mission.ApprovalStatus = "DECLINE";
            _objdb.SaveChanges();
            return true;
        }
        public User EditUser(long UserId)
        {
            return _objdb.Users.Where(u => u.UserId == UserId).FirstOrDefault();
        }

    }
}
