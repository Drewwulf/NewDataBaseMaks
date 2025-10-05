namespace MaksGym.Models
{
    public class Training
    {
        public int TrainingId { get; set; }       // PK
        public int StudentId { get; set; }       
        public int SheduleId { get; set; }  
        public int CoachId { get; set; }
        public int? RoomId { get; set; }
        public DateTime StartTime { get; set; }
        
        public int directionId { get; set; }

        public int subscriptionId { get; set; }
        public bool IsConducted { get; set; } = false;
    }
}
