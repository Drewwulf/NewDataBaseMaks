using System.Text.RegularExpressions;

namespace MaksGym.Models
{
    public class Coach
    {
        public int CoachId { get; set; }            // PK
        public string UserId { get; set; } = null!; // FK -> AspNetUsers.Id
        public string? PhotoPath { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
