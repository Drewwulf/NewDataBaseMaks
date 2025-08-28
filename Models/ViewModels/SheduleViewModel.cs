namespace MaksGym.Models.ViewModels
{
    public class SheduleViewModel
    {
        public int GroupId { get; set; }
        public WeekDay DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int? RoomId { get; set; }
        public List<Room> Rooms { get; set; } = new();

    }
}
