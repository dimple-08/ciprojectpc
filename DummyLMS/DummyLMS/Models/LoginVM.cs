using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DummyLMS.Models
{
    public class LoginVM
    {
        public long id { get; set; }
        [Required]
        public string userName { get; set; }

        [Required]

        public string Password { get; set; }
        public string Role { get; set; }
    }
}