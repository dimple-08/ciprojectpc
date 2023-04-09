using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entity.ViewModels
{
    public class ResetModel
    {
        public string Email { get; set; } = null!;

        public string Token { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;

        public DateTime UpdatedAt { get; set; }
    }
}
