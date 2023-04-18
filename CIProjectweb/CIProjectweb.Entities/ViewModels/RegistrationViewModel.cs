using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class RegistrationViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter FirstName")]
        [RegularExpression("^[^0-9]*$", ErrorMessage="Please Enter Valid Name")]
        public string? FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter LastName")]
        [RegularExpression("^[^0-9]*$", ErrorMessage = "Please Enter Valid Name")]
        public string? LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,3}$", ErrorMessage = "Please Provide Valid Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Field can't be empty")]

        [MinLength(8, ErrorMessage = "Password Must be atleast 8 character")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}", ErrorMessage = "Please Enter Valid Password ")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassowrd { get; set; } = null!;

        [Required(ErrorMessage = "Field can't be empty")]
        [RegularExpression("[789][0-9]{9}", ErrorMessage = "Please Enter Valid Mobile Number")]
        public long PhoneNumber { get; set; }

    }
}
