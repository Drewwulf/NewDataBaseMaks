using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaksGym.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public string UserId { get; set; } = null!; 
        public string? PhotoPath { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0;
        public ApplicationUser? User { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        public float discount { get; set; } 
        public ICollection<StudentToGroup> StudentToGroups { get; set; } = new List<StudentToGroup>();
        public ICollection<StudentsToSubscription> StudentsToSubscriptions { get; set; } = new List<StudentsToSubscription>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
