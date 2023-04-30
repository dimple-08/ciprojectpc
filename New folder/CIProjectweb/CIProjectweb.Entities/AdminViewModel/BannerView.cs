
using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.AdminViewModel
{
    public class BannerView
    {
        public List<Banner> banners = new List<Banner>();
        public long BannerId { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string Image { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        [MinLength(100),MaxLength(500)]
        public string Text { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Sort must be properly formatted.")]
        public int SortOrder { get; set; }
    }
}
