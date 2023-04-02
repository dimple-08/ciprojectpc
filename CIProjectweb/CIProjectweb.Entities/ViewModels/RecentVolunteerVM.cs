using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class RecentVolunteerVM
    {
        public string? missions { get; set; }
        public string? users { get; set;}
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
