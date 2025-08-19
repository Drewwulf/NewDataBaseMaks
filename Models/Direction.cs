using System.ComponentModel.DataAnnotations.Schema;

namespace MaksGym.Models
{
    [Table("Directions")]
    public class Direction
    {
        public int DirectionId { get; set; }                 // PK
        public string DirectionName { get; set; } = null!;
        public string? DirectionDescription { get; set; }
        public bool isDeleted { get; set; } = false;
        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
