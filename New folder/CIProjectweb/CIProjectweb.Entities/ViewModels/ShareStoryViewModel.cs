using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class ShareStoryViewModel
    {
        public List<Mission> missionsList { get; set; }
        public Story? Story { get; set; }
    }
}
