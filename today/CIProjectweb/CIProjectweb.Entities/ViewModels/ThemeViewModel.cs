using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class ThemeViewModel
    {
        public long MissionThemeId { get; set; }

        public string? Title { get; set; }

        public byte Status { get; set; }

        public DateTime? CreatedAt { get; set; } = null!;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

       
    }
}
