using System;

namespace MaksGym.Models
{
    public class SubscriptionFreezeTime
    {
        public int SubscriptionFreezeTimeId { get; set; }   // PK

        public int StudentsToSubscriptionId { get; set; }   // FK -> StudentsToSubscription
        public StudentsToSubscription StudentsToSubscription { get; set; } = null!;

        public DateOnly FreezeStart { get; set; }           // date
        public DateOnly FreezeEnd { get; set; }             // date
    }
}
