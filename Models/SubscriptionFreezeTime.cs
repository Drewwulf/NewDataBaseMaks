using System;

namespace MaksGym.Models
{
    public class SubscriptionFreezeTime
    {
        public int SubscriptionFreezeTimeId { get; set; }   // PK

        public int StudentsToSubscriptionId { get; set; }   // FK -> StudentsToSubscription
        public StudentsToSubscription StudentsToSubscription { get; set; } = null!;

        public DateTime FreezeStart { get; set; }   
        public DateTime? FreezeEnd { get; set; }           
    }
}
