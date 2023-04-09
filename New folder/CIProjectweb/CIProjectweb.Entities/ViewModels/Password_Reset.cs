using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class Password_Reset
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string Token { get; set; } = null!;
    }
}
