using System;
using MaksGym.Models;

public class Schedule
{
    public int SheduleId { get; set; }    // PK
    public int GroupsId { get; set; }      // FK -> Groups.GroupsId
    public int? RoomId { get; set; }     

    public WeekDay DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public Group Group { get; set; } = null!;
    public Room? Room { get; set; }
}

public enum WeekDay
{
    Monday = 1,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}
