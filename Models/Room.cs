using System.ComponentModel.DataAnnotations.Schema;

namespace MaksGym.Models
{
    [Table("Rooms")]
    public class Room
    {
        public int RoomId { get; set; }                       // PK
        public string RoomName { get; set; } = null!;
        public string? RoomDescription { get; set; }

        public ICollection<Shedule> Shedules { get; set; } = new List<Shedule>();
    }
}
