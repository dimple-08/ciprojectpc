using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Models.ViewModels
{
    public class Registration
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int PhoneNumber { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

    }
}
