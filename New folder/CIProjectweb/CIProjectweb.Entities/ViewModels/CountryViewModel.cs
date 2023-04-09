using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class CountryViewModel
    {
        public long CountryId { get; set; }

        public string Name { get; set; } 

        public string Iso { get; set; } 

        public DateTime? CreatedAt { get; set; } 

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

       
    }
}
