using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaksGym.Models
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }      
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }

        public bool IsDeleted { get; set; } = false;

    
        public ICollection<StudentsToSubscription> StudentsToSubscriptions { get; set; } = new List<StudentsToSubscription>();

    }
}
