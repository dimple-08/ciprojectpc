using CIProjectweb.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIProjectweb.Entities.AdminViewModel
{
    public class CMSViewModel
    {
       public List<CmsPage>? cMSViewModels { get; set; }
        public long CmsPageId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string Slug { get; set; } = null!;

        public bool? Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
