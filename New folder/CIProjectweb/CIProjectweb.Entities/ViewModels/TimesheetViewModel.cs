using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class TimesheetViewModel
    {
        public List<SelectListItem> missionstime { get; set; } = null!;
        public List<SelectListItem> missionsgoal { get; set; } = null!;
        public long MissionId { get; set; }
        public long TimesheetId { get; set; }

        public long? UserId { get; set; }

        public string? Time { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public string? Action { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        public DateTime DateVolunteered { get; set; }

       
        public string? Notes { get; set; }

        public string Status { get; set; } = null!;

        public byte[] CreatedAt { get; set; } = null!;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
        public string Title { get; set; } = null!;
        public List<TimesheetViewModel> timesheets { get; set; } = null!;

        public List<TimesheetViewModel> timesheettime { get; set; } = null!;
        [Required(ErrorMessage = "Field can't be empty")]
        [Range(0, 23, ErrorMessage = "Hour must be between 0 and 23")]
      
        public string? Timehour { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [Range(0, 59, ErrorMessage = "Minute must be between 0 and 59")]
        public string? Timeminute { get; set; }
    }
}
