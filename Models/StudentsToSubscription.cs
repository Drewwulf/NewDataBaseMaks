using System;
using System.Collections.Generic;

namespace MaksGym.Models
{
    public class StudentsToSubscription
    {
        public int StudentsToSubscriptionId { get; set; } // PK

        public int StudentId { get; set; }                // FK -> Students.StudentId
        public Student Student { get; set; } = null!;

        public int SubscriptionId { get; set; }           // FK -> Subscriptions.SubscriptionId
        public Subscription Subscription { get; set; } = null!;

        public int? TransactionId { get; set; }           // FK -> Transaction.TransactionId (nullable)
        public Transaction? Transaction { get; set; }

        public DateOnly StartDate { get; set; }           // date

        public ICollection<SubscriptionFreezeTime> Freezes { get; set; }
     = new List<SubscriptionFreezeTime>();

    }
}
