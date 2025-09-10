using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaksGym.Models
{
    [Table("Rooms")]
    public class Room
    {
        public int RoomId { get; set; }   
    [Display(Name = "Введіть назву кімнати")]
        [Required(ErrorMessage ="Введення назви кімнати є обовязкове")]
        public string RoomName { get; set; } = null!;
        [Display(Name = "Введіть опис кімнати")]

        public string? RoomDescription { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Schedule> Shedules { get; set; } = new List<Schedule>();
    }
}
