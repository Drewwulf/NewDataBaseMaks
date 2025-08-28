using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaksGym.Models
{
    [Table("Trainers")]
    public class Coach
    {
        public int CoachId { get; set; }

        [Required(ErrorMessage = "Вкажіть користувача")]
        public string UserId { get; set; } = null!; // FK -> AspNetUsers.Id

        public string? PhotoPath { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ApplicationUser? User { get; set; }

        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
