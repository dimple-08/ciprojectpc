using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.AdminViewModel
{
    public class SkillView
    {
        public List<Skill> skills= new List<Skill>();
        public long SkillId { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [RegularExpression("^((?!^Theme Title)[a-zA-Z '])+$", ErrorMessage = "Title  must be properly formatted.")]
        public string SkillName { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        public string Status { get; set; }
    }
}
