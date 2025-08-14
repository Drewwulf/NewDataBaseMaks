namespace MaksGym.Models
{
    public class Group
    {
        public int GroupsId { get; set; }           // PK
        public int CoachId { get; set; }            // FK -> Coaches.CoachId
        public int DirectionId { get; set; }        // FK -> Directions.DirectionId
        public bool DeletionFlag { get; set; }
        public string GroupName { get; set; } = null!;
        public string? GroupDescription { get; set; }

        public Coach Coach { get; set; } = null!;
        public Direction Direction { get; set; } = null!;
        public ICollection<StudentToGroup> StudentToGroups { get; set; } = new List<StudentToGroup>();

        // 🔹 Додаємо колекцію розкладів
        public ICollection<Shedule> Shedules { get; set; } = new List<Shedule>();
    }
}
