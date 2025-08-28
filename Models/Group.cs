namespace MaksGym.Models
{
    public class Group
    {
        public int GroupsId { get; set; }           // PK
        public int CoachId { get; set; }            // FK -> Coaches.CoachId
        public int DirectionId { get; set; }        // FK -> Directions.DirectionId
        public bool IsDeleted { get; set; } = false;
        public string GroupName { get; set; } = null!;
        public string? GroupDescription { get; set; }

        public Coach? Coach { get; set; } = null!;
        public Direction? Direction { get; set; } = null!;
        public ICollection<StudentToGroup> StudentToGroups { get; set; } = new List<StudentToGroup>();

        public ICollection<Schedule> Shedules { get; set; } = new List<Schedule>();
    }
}
