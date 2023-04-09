using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entity.ViewModels
{
    public class ForgotModel
    {
        [Required(ErrorMessage = "please enter Email")]
        public string? Email { get; set; }
    }
}
