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
    public class Admin : IAdmin
    {
        private readonly CIDbContext _objdb;
        public Admin(CIDbContext objdb)
        {

            _objdb = objdb;
        }

        public List<User> alluser()
        {
            List<User> users = _objdb.Users.Where(u => u.DeletedAt == null).ToList();
            return users;
        }
        public bool AddUser(string email)
        {
            var userexsists = _objdb.Users.Where(a => a.Email.Equals(email) && a.DeletedAt == null && a.Status == true).FirstOrDefault();
            if (userexsists == null)
            {
                return true;
            }
            else
            {
                return false;
            }
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
        public bool AddUser(string FirstName, string Lastname, string Email, string EmployeeId, string Password, string Department, long City, long Country, string Profile, string Status, string Avtar, long UserId)
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
                    userAdd.Avatar = "~/images/user1.png";
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
                User user = _objdb.Users.Where(x => x.UserId == UserId).FirstOrDefault();
                if (user != null)
                {
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
                    if (Avtar == null)
                    {
                        user.Avatar = "~/images/user1.png";
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
                return false;
            }

        }
        public void deleteuser(long userId)
        {
            var user = _objdb.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.DeletedAt = DateTime.Now;
                user.Status = false;
                _objdb.Users.Update(user);
                _objdb.SaveChanges();
            }

        }
        public void deleteCMS(long PageId)
        {
            var user = _objdb.CmsPages.FirstOrDefault(u => u.CmsPageId == PageId);

            if (user != null)
            {
                user.DeletedAt = DateTime.Now;
                _objdb.CmsPages.Update(user);
                _objdb.SaveChanges();
            }
        }
        public List<Story> allstory()
        {
            List<Story> stories = _objdb.Stories.Where(st => st.Status != "PUBLISH" && st.DeletedAt == null).ToList();
            return stories;
        }

        public bool ADDCms(CMSViewModel cmsadd)
        {

            if (cmsadd.CmsPageId == 0)
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
            List<MissionApplication> missionApplications = _objdb.MissionApplications.Where(ma => ma.ApprovalStatus == "PENDING").ToList();
            return missionApplications;
        }
        public CMSViewModel cmsrecordall()
        {
            CMSViewModel cms = new CMSViewModel();
            cms.cMSViewModels = _objdb.CmsPages.Where(cms => cms.DeletedAt == null).ToList();
            return cms;
        }
        public List<Mission> allmission()
        {
            List<Mission> missions = _objdb.Missions.Where(m => m.DeletedAt == null).ToList();
            return missions;
        }
        public bool Approve(long applicationId)
        {
            MissionApplication mission = _objdb.MissionApplications.Where(ma => ma.MissionApplicationId == applicationId).FirstOrDefault();
            mission.ApprovalStatus = "ACCEPT";
            mission.UpdatedAt = DateTime.Now;
            _objdb.MissionApplications.Update(mission);
            _objdb.SaveChanges();
            Notification addnotification = new Notification();
            addnotification.UserId = mission.UserId;
            addnotification.NotificationText = "Your Mission Application is Accepted.";
            _objdb.Notifications.Add(addnotification);
            _objdb.SaveChanges();
            return true;
        }
        public bool Decline(long applicationId)
        {
            MissionApplication mission = _objdb.MissionApplications.Where(ma => ma.MissionApplicationId == applicationId).FirstOrDefault();
            mission.ApprovalStatus = "DECLINE";
            mission.UpdatedAt = DateTime.Now;
            _objdb.MissionApplications.Update(mission);
            _objdb.SaveChanges();
            Notification addnotification = new Notification();
            addnotification.UserId = mission.UserId;
            addnotification.NotificationText = "Your Mission Application is Declined.";
            _objdb.Notifications.Add(addnotification);
            _objdb.SaveChanges();
            return true;
        }
        public User EditUser(long UserId)
        {
            return _objdb.Users.Where(u => u.UserId == UserId).FirstOrDefault();
        }
        public List<MissionTheme> alltheme()
        {
            List<MissionTheme> list = _objdb.MissionThemes.Where(mt => mt.DeletedAt == null && mt.Status == 1).ToList();
            return list;
        }
        public List<MissionTheme> allthemes()
        {
            List<MissionTheme> list = _objdb.MissionThemes.Where(mt => mt.DeletedAt == null).ToList();
            return list;
        }
        public List<GoalMission> allgoalmissions()
        {
            List<GoalMission> list = _objdb.GoalMissions.Where(mt => mt.DeletedAt == null).ToList();
            return list;
        }
        public MissionTheme themeedit(long themeId)
        {
            var theme = _objdb.MissionThemes.FirstOrDefault(mt => mt.MissionThemeId == themeId);
            return theme;
        }
        public bool addtheme(ThemeView themeView)
        {
            var theme = _objdb.MissionThemes.FirstOrDefault(mt => mt.MissionThemeId == themeView.MissionThemeId);
            if (theme != null)
            {
                theme.Title = themeView.Title;
                if (themeView.Status == "Active")
                {
                    theme.Status = 1;
                }
                else
                {
                    theme.Status = 0;
                }
                _objdb.MissionThemes.Update(theme);
                _objdb.SaveChanges();
                return false;
            }
            else
            {
                MissionTheme missionTheme = new MissionTheme();
                missionTheme.Title = themeView.Title;
                if (themeView.Status == "Active")
                {
                    missionTheme.Status = 1;
                }
                else
                {
                    missionTheme.Status = 0;
                }
                _objdb.MissionThemes.Add(missionTheme);
                _objdb.SaveChanges();
                return true;
            }
        }
        public void approve(long storyId)
        {
            var story = _objdb.Stories.FirstOrDefault(st => st.StoryId == storyId);
            if (story != null)
            {
                story.Status = "PUBLISH";
                story.UpdatedAt = DateTime.Now;
                _objdb.Stories.Update(story);
                _objdb.SaveChanges();
                Notification addnotification = new Notification();
                addnotification.UserId = story.UserId;
                addnotification.NotificationText = "Your Story is approved";
                _objdb.Notifications.Add(addnotification);
                _objdb.SaveChanges();
            }
        }
        public void reject(long storyId)
        {
            var story = _objdb.Stories.FirstOrDefault(st => st.StoryId == storyId);
            if (story != null)
            {
                story.Status = "DECLINE";
                story.UpdatedAt = DateTime.Now;
                _objdb.Stories.Update(story);
                _objdb.SaveChanges();
                Notification addnotification = new Notification();
                addnotification.UserId = story.UserId;
                addnotification.NotificationText = "Your Story is Rejected.";
                _objdb.Notifications.Add(addnotification);
                _objdb.SaveChanges();
            }
        }

        public void delete(long storyId)
        {
            var story = _objdb.Stories.FirstOrDefault(st => st.StoryId == storyId);
            if (story != null)
            {

                story.DeletedAt = DateTime.Now;
                _objdb.Stories.Update(story);
                _objdb.SaveChanges();
            }
        }
        public bool deletetheme(long themeId)
        {
            List<DateTime> endDates = new List<DateTime>();
            List<Mission> missions = _objdb.Missions.Where(ms => ms.ThemeId == themeId).ToList();

            foreach (var mission in missions)
            {

                if (mission.EndDate > DateTime.Now)
                {

                    endDates.Add((DateTime)mission.EndDate);
                }

            }

            if (endDates.Count() >= 1)
            {
                return false;
            }



            else
            {
                List<Mission> missions1 = _objdb.Missions.Where(ms => ms.ThemeId == themeId).ToList();
                if (missions1.Count > 0)
                {
                    var find = _objdb.MissionThemes.FirstOrDefault(mt => mt.MissionThemeId == themeId);
                    find.DeletedAt = DateTime.Now;
                    _objdb.MissionThemes.Update(find);
                    _objdb.SaveChanges();
                    foreach (var mission in missions1)
                    {
                        mission.DeletedAt = DateTime.Now;
                        mission.Status = false;
                        _objdb.Missions.Update(mission);
                        _objdb.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    var find = _objdb.MissionThemes.FirstOrDefault(mt => mt.MissionThemeId == themeId);
                    find.DeletedAt = DateTime.Now;
                    _objdb.MissionThemes.Update(find);
                    _objdb.SaveChanges();
                    return true;
                }


            }
            //List<Mission> missions = _objdb.Missions.Where(ms => ms.ThemeId == themeId).ToList();
            //if (missions.Count > 0)
            //{
            //    var find = _objdb.MissionThemes.FirstOrDefault(mt => mt.MissionThemeId == themeId);
            //    find.DeletedAt = DateTime.Now;
            //    _objdb.MissionThemes.Update(find);
            //    _objdb.SaveChanges();
            //    foreach (var mission in missions)
            //    {
            //        mission.DeletedAt= DateTime.Now;
            //        mission.Status = false;
            //        _objdb.Missions.Update(mission);
            //        _objdb.SaveChanges();
            //    }
            //}
            //else
            //{
            //    var find = _objdb.MissionThemes.FirstOrDefault(mt => mt.MissionThemeId == themeId);
            //    find.DeletedAt = DateTime.Now;
            //    _objdb.MissionThemes.Update(find);
            //    _objdb.SaveChanges();
            //}
        }
        public List<Banner> bannerlist()
        {
            var list = _objdb.Banners.Where(b => b.DeletedAt == null).ToList();
            return list;
        }
        public Banner banner(long bannerId)
        {
            var banner = _objdb.Banners.FirstOrDefault(b => b.BannerId == bannerId);
            return banner;
        }
        public bool bannersave(CIProjectweb.Entities.AdminViewModel.BannerView bannerView)
        {
            var banner = _objdb.Banners.FirstOrDefault(b => b.BannerId == bannerView.BannerId);
            if (banner != null)
            {
                banner.Text = bannerView.Text;
                banner.SortOrder = bannerView.SortOrder;
                banner.Image = bannerView.Image;
                _objdb.Banners.Update(banner);
                _objdb.SaveChanges();
                return false;
            }
            else
            {
                Banner banner1 = new Banner();
                banner1.Text = bannerView.Text;
                banner1.SortOrder = bannerView.SortOrder;
                banner1.Image = bannerView.Image;
                _objdb.Banners.Add(banner1);
                _objdb.SaveChanges();
                return true;
            }

        }
        public void deletebanner(long bannerId)
        {
            var banner = _objdb.Banners.FirstOrDefault(b => b.BannerId == bannerId);
            if (banner != null)
            {
                banner.DeletedAt = DateTime.Now;
                _objdb.Banners.Update(banner);
                _objdb.SaveChanges();
            }
        }
        public List<Skill> skilllist()
        {
            List<Skill> list = _objdb.Skills.Where(sk => sk.DeletedAt == null && sk.Status == 1).ToList();
            return list;
        }
        public List<Skill> skillls()
        {
            List<Skill> list = _objdb.Skills.Where(sk => sk.DeletedAt == null).ToList();
            return list;
        }
        public Skill skilledit(long skillId)
        {
            var skill = _objdb.Skills.FirstOrDefault(sk => sk.SkillId == skillId);
            return skill;
        }
        public bool deleteskill(long skillId)
        {

            List<DateTime> endDates = new List<DateTime>();
            var missionList = _objdb.MissionSkills.Where(MS => MS.SkillId == skillId).ToList();
            var allMissions = _objdb.Missions.ToList();
            foreach (var missionSkill in missionList)
            {
                foreach (var mission in allMissions)
                {
                    if (missionSkill.MissionId == mission.MissionId)
                    {
                        if (mission.EndDate > DateTime.Now)
                        {

                            endDates.Add((DateTime)mission.EndDate);
                        }
                    }
                }
            }
            if (endDates.Count() >= 1)
            {
                return false;
            }



            else
            {

                var skill = _objdb.Skills.Where(s => s.SkillId == skillId).FirstOrDefault();

                skill.DeletedAt = DateTime.Now;
                skill.Status = 0;
                _objdb.SaveChanges();
                return true;
            }

        }
        public bool skillsave(CIProjectweb.Entities.AdminViewModel.SkillView skillView)
        {
            var skill = _objdb.Skills.FirstOrDefault(sk => sk.SkillId == skillView.SkillId);
            if (skill != null)
            {
                skill.SkillName = skillView.SkillName;
                skill.UpdatedAt = DateTime.Now;
                if (skillView.Status == "Active")
                {
                    skill.Status = 1;
                }
                else
                {
                    skill.Status = 0;
                }
                _objdb.Skills.Update(skill);
                _objdb.SaveChanges();
                return false;
            }
            else
            {
                Skill skill1 = new Skill();
                skill1.SkillName = skillView.SkillName;
                if (skillView.Status == "Active")
                {
                    skill1.Status = 1;
                }
                else
                {
                    skill1.Status = 0;
                }
                _objdb.Skills.Add(skill1);
                _objdb.SaveChanges();
                return true;
            }
        }
        public bool saveuser(UserView userView)
        {
            var user = _objdb.Users.FirstOrDefault(u => u.UserId == userView.UserId);
            if (user != null)
            {
                user.FirstName = userView.FirstName;
                user.LastName = userView.LastName;
                user.Email = userView.Email;
                user.Password = userView.Password;
                user.EmployeeId = userView.EmployeeId;
                user.Department = userView.Department;
                user.CityId = userView.CityId;
                user.CountryId = userView.CountryId;
                user.ProfileText = userView.ProfileText;
                if (userView.Status == "Active")
                {
                    user.Status = true;
                }
                else
                {
                    user.Status = false;
                }
                if (userView.Avatar == null)
                {
                    user.Avatar = "/images/user1.png";
                }
                else
                {
                    user.Avatar = userView.Avatar;
                }

                _objdb.Users.Update(user);
                _objdb.SaveChanges();
                return false;
            }
            else
            {
                User user1 = new User();
                user1.FirstName = userView.FirstName;
                user1.LastName = userView.LastName;
                user1.Email = userView.Email;
                user1.Password = userView.Password;
                user1.EmployeeId = userView.EmployeeId;
                user1.Department = userView.Department;
                user1.CityId = userView.CityId;
                user1.CountryId = userView.CountryId;
                user1.ProfileText = userView.ProfileText;
                if (userView.Status == "Active")
                {
                    user1.Status = true;
                }
                else
                {
                    user1.Status = false;
                }
                if (userView.Avatar == null)
                {
                    user1.Avatar = "/images/user1.png";
                }
                else
                {
                    user1.Avatar = userView.Avatar;
                }

                _objdb.Users.Add(user1);
                _objdb.SaveChanges();
                return true;
            }
        }
        public bool savemission(MissionView missionView, string[] selectedValues, string[] dataUrls, string[] docFiles, string[] docName, string videoUrls)
        {
            var findmission = _objdb.Missions.FirstOrDefault(m => m.MissionId == missionView.MissionId);
            int d = 0;
            if (findmission != null)
            {
                findmission.Title = missionView.Title;
                findmission.ThemeId = missionView.ThemeId;
                findmission.CityId = missionView.CityId;
                findmission.CountryId = missionView.CountryId;
                findmission.Description = missionView.Description;
                findmission.ShortDescription = missionView.ShortDescription;
                findmission.StartDate = missionView.StartDate;
                findmission.EndDate = missionView.EndDate == null ? null : missionView.EndDate;
                findmission.MissionType = missionView.MissionType;
                if (missionView.MissionType == "Goal")
                {
                    GoalMission goal = _objdb.GoalMissions.Where(g => g.MissionId == findmission.MissionId).FirstOrDefault();
                    if (goal != null)
                    {
                        goal.GoalObjectiveText = missionView.GoalObjectiveText;
                        goal.GoalValue = (int)missionView.GoalValue;
                    }


                }
                findmission.OrganizationName = missionView.OrganizationName;
                findmission.OrganizationDetail = missionView.OrganizationDetalis;
                findmission.Availability = missionView.Availability;
                findmission.SeatAvailable = missionView.SeatsAvailable.ToString();
                findmission.Deadline = missionView.Deadline;
                findmission.UpdatedAt = DateTime.Now;
                findmission.Status = true;
                _objdb.Missions.Update(findmission);
                List<MissionSkill> missionSkills = _objdb.MissionSkills.Where(ms => ms.MissionId == findmission.MissionId).ToList();
                foreach (var item in missionSkills)
                {
                    _objdb.MissionSkills.Remove(item);
                }
                foreach (var item in selectedValues)
                {
                    MissionSkill missionSkill = new MissionSkill();
                    missionSkill.MissionId = findmission.MissionId;
                    missionSkill.SkillId = long.Parse(item);
                    _objdb.Add(missionSkill);
                }
                List<MissionMedium> missionMedia = _objdb.MissionMedia.Where(mm => mm.MissionId == findmission.MissionId).ToList();
                foreach (var item in missionMedia)
                {
                    _objdb.MissionMedia.Remove(item);
                }
                foreach (var item in dataUrls)
                {
                    d++;

                    MissionMedium missionMedium = new MissionMedium();
                    if (d == 1)
                    {
                        missionMedium.Default = true;
                    }
                    missionMedium.MissionId = findmission.MissionId;
                    missionMedium.MediaPath = item;
                    missionMedium.MediaName = "logo";
                    missionMedium.MediaType = "imag";
                    _objdb.Add(missionMedium);
                }
                d = 0;
                if (videoUrls != null)
                {
                    MissionMedium missionMedium1 = new MissionMedium();
                    missionMedium1.MissionId = findmission.MissionId;
                    missionMedium1.MediaPath = videoUrls;
                    missionMedium1.MediaName = "logo";
                    missionMedium1.MediaType = "video";
                    _objdb.Add(missionMedium1);
                    _objdb.SaveChanges();
                }

                List<MissionDocument> missionDocuments = _objdb.MissionDocuments.Where(md => md.MissionId == findmission.MissionId).ToList();
                foreach (var item in missionDocuments)
                {
                    _objdb.MissionDocuments.Remove(item);
                }
                for (int i = 0; i < docFiles.Length; i++)
                {
                    var file = docFiles[i];
                    var name = docName[i];
                    MissionDocument missionDocument = new MissionDocument();
                    missionDocument.MissionId = findmission.MissionId;
                    missionDocument.DocumentName = name;
                    missionDocument.DocumentPath = file;
                    missionDocument.DocumentType = "document";
                    _objdb.MissionDocuments.Add(missionDocument);
                }
                _objdb.SaveChanges();
                return false;
            }
            else
            {
                Mission mission = new Mission();
                mission.Title = missionView.Title;
                mission.ThemeId = missionView.ThemeId;
                mission.CityId = missionView.CityId;
                mission.CountryId = missionView.CountryId;
                mission.Description = missionView.Description;
                mission.ShortDescription = missionView.ShortDescription;
                mission.StartDate = missionView.StartDate;
                mission.EndDate = missionView.EndDate;
                mission.MissionType = missionView.MissionType;
                mission.OrganizationName = missionView.OrganizationName;
                mission.OrganizationDetail = missionView.OrganizationDetalis;
                mission.Availability = missionView.Availability;
                mission.SeatAvailable = missionView.SeatsAvailable.ToString();
                mission.Deadline = missionView.Deadline;
                mission.Status = true;
                _objdb.Missions.Add(mission);
                _objdb.SaveChanges();
                if (missionView.MissionType == "Goal")
                {
                    GoalMission goal = _objdb.GoalMissions.Where(g => g.MissionId == mission.MissionId).FirstOrDefault();
                    if (goal != null)
                    {
                        goal.GoalObjectiveText = missionView.GoalObjectiveText;
                        goal.GoalValue = missionView.GoalValue;
                    }
                    else
                    {
                        GoalMission goalnew = new GoalMission();
                        goalnew.MissionId = mission.MissionId;
                        goalnew.GoalValue = missionView.GoalValue;
                        goalnew.GoalObjectiveText = missionView.GoalObjectiveText;
                        _objdb.GoalMissions.Add(goalnew);
                        _objdb.SaveChanges();
                    }


                }
                foreach (var item in selectedValues)
                {
                    MissionSkill missionSkill = new MissionSkill();
                    missionSkill.MissionId = mission.MissionId;
                    missionSkill.SkillId = long.Parse(item);
                    _objdb.MissionSkills.Add(missionSkill);
                    _objdb.SaveChanges();
                }
                foreach (var item in dataUrls)
                {
                    d++;

                    MissionMedium missionMedium = new MissionMedium();
                    if (d == 1)
                    {
                        missionMedium.Default = true;
                    }
                    missionMedium.MissionId = mission.MissionId;
                    missionMedium.MediaPath = item;
                    missionMedium.MediaName = "logo";
                    missionMedium.MediaType = "imag";
                    _objdb.Add(missionMedium);
                }
                d = 0;
                if (videoUrls != null)
                {
                    MissionMedium missionMedium1 = new MissionMedium();
                    missionMedium1.MissionId = mission.MissionId;
                    missionMedium1.MediaPath = videoUrls;
                    missionMedium1.MediaName = "logo";
                    missionMedium1.MediaType = "video";
                    _objdb.MissionMedia.Add(missionMedium1);
                    _objdb.SaveChanges();
                }

                for (int i = 0; i < docFiles.Length; i++)
                {
                    var file = docFiles[i];
                    var name = docName[i];
                    MissionDocument missionDocument = new MissionDocument();
                    missionDocument.MissionId = mission.MissionId;
                    missionDocument.DocumentName = name;
                    missionDocument.DocumentPath = file;
                    missionDocument.DocumentType = "document";
                    _objdb.MissionDocuments.Add(missionDocument);
                }
                _objdb.SaveChanges();
                return true;
            }

        }
        public MissionView findmission(long missionId)
        {
            MissionView missionView = new MissionView();
            var mission = _objdb.Missions.FirstOrDefault(m => m.MissionId == missionId);
            if (mission != null)
            {
                missionView.MissionId = missionId;
                missionView.CityId = mission.CityId;
                missionView.CountryId = mission.CountryId;
                missionView.ThemeId = mission.ThemeId;
                missionView.Title = mission.Title;
                missionView.Description = mission.Description;
                missionView.ShortDescription = mission.ShortDescription;
                missionView.StartDate = mission.StartDate;
                missionView.EndDate = mission.EndDate;
                missionView.Deadline = mission.Deadline;
                missionView.SeatsAvailable = int.Parse(mission.SeatAvailable);
                missionView.MissionType = mission.MissionType;
                if (mission.MissionType == "Goal")
                {
                    GoalMission goal = _objdb.GoalMissions.Where(g => g.MissionId == mission.MissionId).FirstOrDefault();
                    if (goal != null)
                    {
                        missionView.GoalObjectiveText = goal.GoalObjectiveText;
                        missionView.GoalValue = goal.GoalValue;
                    }
                    else
                    {
                        missionView.GoalObjectiveText = null;
                        missionView.GoalValue = 0;
                    }


                }
                missionView.OrganizationName = mission.OrganizationName;
                missionView.OrganizationDetalis = mission.OrganizationDetail;
                missionView.Availability = mission.Availability;
                List<MissionSkill> missionSkills = _objdb.MissionSkills.Where(ms => ms.MissionId == missionId && ms.DeletedAt == null).ToList();
                missionView.missionSkill = missionSkills;
                List<MissionMedium> missionMedia = _objdb.MissionMedia.Where(mm => mm.MissionId == missionId && mm.DeletedAt == null).ToList();
                missionView.missionMedia = missionMedia;
                List<MissionDocument> missionDocuments = _objdb.MissionDocuments.Where(md => md.MissionId == missionId && md.DeletedAt == null).ToList();
                missionView.missionDocuments = missionDocuments;
            }
            return missionView;
        }
        #region CountryByCityAdmin

        public List<City> CountryByCityAdmin(long CountryId)
        {
            var city = _objdb.Cities.Where(c => c.CountryId == CountryId).ToList();
            List<City> model = new List<City>();
            model = city;

            return model;


        }


        #endregion
        public bool deletemission(long missionId)
        {
            var findmission = _objdb.Missions.FirstOrDefault(m => m.MissionId == missionId);

            if (findmission != null)
            {
                if (findmission.EndDate > DateTime.Now)
                {
                    var applicationUser = _objdb.MissionApplications.Where(ma => ma.MissionId == findmission.MissionId).ToList();
                    if (applicationUser.Count > 0)
                    {
                        return false;
                    }
                    return false;
                }
                else
                {
                    findmission.DeletedAt = DateTime.Now;
                    findmission.Status = false;
                    _objdb.Missions.Update(findmission);
                    List<MissionSkill> missionSkills = _objdb.MissionSkills.Where(ms => ms.MissionId == missionId).ToList();
                    foreach (var item in missionSkills)
                    {
                        item.DeletedAt = DateTime.Now;
                        _objdb.MissionSkills.Update(item);
                    }
                    List<MissionMedium> missionMedia = _objdb.MissionMedia.Where(mm => mm.MissionId == missionId).ToList();
                    foreach (var item in missionMedia)
                    {
                        item.DeletedAt = DateTime.Now;
                        _objdb.MissionMedia.Update(item);
                    }
                    List<MissionDocument> missionDocuments = _objdb.MissionDocuments.Where(md => md.MissionId == missionId).ToList();
                    foreach (var item in missionDocuments)
                    {
                        item.DeletedAt = DateTime.Now;
                        _objdb.MissionDocuments.Update(item);
                    }
                    _objdb.SaveChanges();
                    return true;
                }

            }
            else
            {
                return false;
            }
        }
        public bool SkillExists(string Title, long skillID)
        {
            var skill = _objdb.Skills.Any(t => t.SkillName == Title && t.DeletedAt == null && t.Status == 1);
            return skill;
        }

        public bool ThemeExists(string Title, long themeID)
        {
            var theme = _objdb.MissionThemes.Any(t => t.Title == Title && t.DeletedAt == null && t.Status==1);
            return theme;
        }

        public bool BannerExists(int order, long bannerId)
        {
            var banner = _objdb.Banners.Any(t => t.SortOrder == order&& t.DeletedAt==null);
            return banner;
        }
        public bool MissionExists(string Title, long missionID)
        {
            var banner = _objdb.Missions.Any(t => t.Title == Title && t.DeletedAt == null);
            return banner;
        }

        public bool EmailExists(string Email, long User)
        {
            var Users = _objdb.Users.Any(t => t.Email == Email && t.DeletedAt == null);
            return Users;
        }
    }
}
