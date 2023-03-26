using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class ResetPAsswordViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,3}$", ErrorMessage = "Please Provide Valid Email")]
        public string Email { get; set; } = null!;

        [Required]
        
        public string Password { get; set; } = null!;

        public string Token { get; set; } = null!;

        [Required]
        [Compare("Password",ErrorMessage ="Don't match with Password.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
