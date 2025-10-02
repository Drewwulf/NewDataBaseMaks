using System;
using System.Collections.Generic;

namespace MaksGym.Models
{
    public class StudentsToSubscription
    {
        public int StudentsToSubscriptionId { get; set; }

        public int StudentId { get; set; }            
        public Student Student { get; set; } = null!;

        public int SubscriptionId { get; set; }        
        public Subscription Subscription { get; set; } = null!;

        public int ActiveSessions { get; set; }

        public int? TransactionId { get; set; }         
        public Transaction? Transaction { get; set; }

        public DateTime StartDate { get; set; }         
        public DateTime EndDate { get; set; }           
        public ICollection<SubscriptionFreezeTime> Freezes { get; set; }
     = new List<SubscriptionFreezeTime>();
        public bool IsFrozen => Freezes.Any(f => f.FreezeEnd == null);




    }
}
