

using Microsoft.AspNetCore.Mvc.Rendering;


namespace CIProjectweb.Entities.ViewModels
{
    public class MissionListingModel
    {
        public List<MissionCardModel> missionsCard { get; set; } = null!;

        public List<SelectListItem> themes { get; set; } = null!;

        public List<SelectListItem> cities { get; set; } = null!;

        public List<SelectListItem> countries { get; set; } = null!;

        public List<SelectListItem> skills { get; set; } = null!;

        public int? TotalMission { get; set; }

    }
}
