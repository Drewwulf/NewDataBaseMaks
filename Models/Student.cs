namespace MaksGym.Models
{
    public class Student
    {
        public int StudentId { get; set; }          // PK
        public string UserId { get; set; } = null!; // FK -> AspNetUsers.Id
        public string? PhotoPath { get; set; }      // шлях до фото

        public ApplicationUser User { get; set; } = null!;
        public ICollection<StudentToGroup> StudentToGroups { get; set; } = new List<StudentToGroup>();
        public ICollection<StudentsToSubscription> StudentsToSubscriptions { get; set; } = new List<StudentsToSubscription>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
