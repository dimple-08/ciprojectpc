using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CIProjectweb.Entities.AdminViewModel
{
    public class UserView
    {
        public List<User> userdata = new List<User>();
        public long UserId { get; set; }
        [RegularExpression("^((?!^Theme Title)[a-zA-Z '])+$", ErrorMessage = "FirstName  must be properly formatted.")]
        [Required(ErrorMessage = "Field can't be empty")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        [RegularExpression("^((?!^Theme Title)[a-zA-Z '])+$", ErrorMessage = "LastName  must be properly formatted.")]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,3}$", ErrorMessage = "Please Provide Valid Email")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        [MinLength(8, ErrorMessage = "Password Must be atleast 8 character")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}", ErrorMessage = "Please Enter Valid Password ")]
        public string Password { get; set; } = null!;
        [RegularExpression("[789][0-9]{9}", ErrorMessage = "Please Enter Valid Mobile Number")]
        [Required(ErrorMessage = "Field can't be empty")]
        public int PhoneNumber { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? Avatar { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [MinLength(100), MaxLength(500)]
        public string? WhyIVolunteer { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? EmployeeId { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? Department { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? ManagerDetail { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public long? CityId { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public long? CountryId { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? ProfileText { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? LinkedInUrl { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? Availability { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? Status { get; set; }
    }
}
