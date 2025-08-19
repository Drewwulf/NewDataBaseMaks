using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaksGym.Models
{
    [Table("Rooms")]
    public class Room
    {
        public int RoomId { get; set; }   
        // PK
    [Display(Name = "Введіть назву кімнати")]
        [Required(ErrorMessage ="Введення назви кімнати є обовязкове")]
        public string RoomName { get; set; } = null!;
        [Display(Name = "Введіть опис кімнати")]

        public string? RoomDescription { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Shedule> Shedules { get; set; } = new List<Shedule>();
    }
}
