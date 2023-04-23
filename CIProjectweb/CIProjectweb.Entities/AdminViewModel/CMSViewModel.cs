using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.AdminViewModel
{
    public class CMSViewModel
    {
       public List<CmsPage>? cMSViewModels { get; set; }
        public long CmsPageId { get; set; }

        
        [DataType(DataType.Text)]
        [Display(Order = 1, Name = "ThemeTitle")]
        [RegularExpression("^((?!^Theme Title)[a-zA-Z '])+$", ErrorMessage = "Title  must be properly formatted.")]
        [Required(ErrorMessage = "Field can't be empty")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        public string Slug { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        public bool? Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
