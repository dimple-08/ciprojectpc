using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class CityViewModel
    {
        public long CityId { get; set; }

        public long CountryId { get; set; }

        public string Name { get; set; } = null!;

        public DateTime? CreatedAt { get; set; } = null!;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

       
    }
}
