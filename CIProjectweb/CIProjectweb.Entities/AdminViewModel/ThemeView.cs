using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.AdminViewModel
{
    public class ThemeView
    {
        public List<MissionTheme> missionThemes { get; set; }
        public long MissionThemeId { get; set; }

        public string Title { get; set; } = null!;

        public string Status { get; set; }
    }
}
