using CIProjectweb.Entities.DataModels;


namespace CIProjectweb.Entities.ViewModels

{
    public class MissionCardModel
    {
        public Mission mission { get; set; } = null!;

        public string? CardImg { get; set; }

        public int? missionRating { get; set; }

        public int? seatsLeft { get; set; }
        public int? alreadyVolunteered { get; set; }

        public int? missionApplied { get; set; }

        public int? approvalPending { get; set; }
        public int? favMission { get; set; }

        public GoalMission? goalMission { get; set; }

        public float? progressBar { get; set; }

        public string theme { get; set; } = null!;

        public string country { get; set; } = null!;

        public float? avgRating { get; set; }


    }
}
