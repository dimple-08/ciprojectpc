using CIProjectweb.Entities.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class Userviewmodel
    {
        public long UserId { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Field can't be empty")]
        [MinLength(8, ErrorMessage = "Password Must be atleast 8 character")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}", ErrorMessage = "Please Enter Valid Password ")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Field can't be empty")]
        [MinLength(8, ErrorMessage = "Password Must be atleast 8 character")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}", ErrorMessage = "Please Enter Valid Password ")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string confirmPasswrd { get; set; } = null!;

        public int PhoneNumber { get; set; }

        public string? Avatar { get; set; }

        public string? WhyIVolunteer { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? EmployeeId { get; set; }

        public string? Department { get; set; }
        public string? ManagerDetail { get; set; }

        public long? CityId { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        public long? CountryId { get; set; }

        public string? ProfileText { get; set; }

        public string? LinkedInUrl { get; set; }

        public string? Title { get; set; }
        public string? Availability { get; set; }

        public bool? Status { get; set; }

        public byte[] CreatedAt { get; set; } = null!;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }


        public long SkillId { get; set; }

        public string SkillName { get; set; } = null!;
        public List<SelectListItem> cities { get; set; } = null!;
        public List<SelectListItem> countries { get; set; } = null!;
        public List<SelectListItem> skills { get; set; } = null!;
        public List<SelectListItem> userskill { get; set; } = null!;
    }
}
