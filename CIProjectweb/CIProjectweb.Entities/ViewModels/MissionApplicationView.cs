using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.ViewModels
{
    public class MissionApplicationView
    {
        public string Title { get; set; } = null!;
        public long MissionId { get; set; }
        public long UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long? applicationId { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
