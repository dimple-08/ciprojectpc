﻿using CIProjectweb.Entities.AdminViewModel;
using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Repository.Repository.Interface
{
    public interface IAdmin
    {
        public List<User> alluser();

        public bool AddUser(string FirstName, string Lastname, string Email, string EmployeeId, string Password, string Department, long City, long Country, string Profile, string Status, string Avtar, long UserId);
        public void deleteuser(long userId);
        public List<City> cities();
        public bool ADDCms(CMSViewModel cmsadd);
        public User EditUser(long UserId);
        public List<Country> countries();
        public List<Mission> allmission();
        public CMSViewModel cmsrecordall();
        public List<MissionApplication> allmissionapp();
        public List<Story> allstory();
        public CmsPage EditCMS(long CMSPAgeID);
        public bool Approve(long applicationId);
        public bool Decline(long applicationId);
        public List<MissionTheme> alltheme();
        public MissionTheme themeedit(long themeId);
        public bool addtheme(ThemeView themeView);
        public void deletetheme(long themeId);

        public void deleteCMS(long PageId);
        public void reject(long storyId);
        public void approve(long storyId);
        public void delete(long storyId);
        public List<Banner> bannerlist();

        public Banner banner(long bannerId);

        public bool bannersave(CIProjectweb.Entities.AdminViewModel.BannerView bannerView);
        public void deletebanner(long bannerId);


        public List<Skill> skilllist();
        public List<GoalMission> allgoalmissions();
        public List<City> CountryByCityAdmin(long CountryId);
        public Skill skilledit(long skillId);

        public bool deleteskill(long skillId);

        public void skillsave(CIProjectweb.Entities.AdminViewModel.SkillView skillView);
        public MissionView findmission(long missionId);
        public void deletemission(long missionId);

        public void savemission(MissionView missionView, string[] selectedValues, string[] dataUrls, string[] docFiles, string[] docName, string videoUrls);
    }
}
