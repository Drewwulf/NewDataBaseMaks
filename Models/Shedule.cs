using MaksGym.Models;

public class Shedule
{
    public int SheduleId { get; set; }    // PK
    public int GroupsId { get; set; }     // FK -> Groups.GroupsId
    public int RoomId { get; set; }       // FK -> Rooms.RoomId
    public int DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public Group Group { get; set; } = null!;
    public Room Room { get; set; } = null!;
}
