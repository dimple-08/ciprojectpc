using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class MailViewModel
    {
        public long userId { get; set; }
        public string Name { get; set; }
        public long email { get; set; }
        public long missionId { get; set; }

       
    }
}
