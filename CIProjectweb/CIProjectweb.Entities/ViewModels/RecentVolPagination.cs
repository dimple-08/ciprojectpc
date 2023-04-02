using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class RecentVolPagination
    {
        public List<RecentVolunteerVM> Items { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public long MissionId { get; set; }
    }
}
