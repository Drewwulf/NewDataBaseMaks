namespace MaksGym.Models.ViewModels
{
    public class StudentDetailsViewModel
    {
        public Student NewStudent { get; set; }
       public List<Subscription> Subscriptions { get; set; }
        public List<Subscription> subscriptions { get; set; }
        public int SubscriptionId { get; set; }
        public int GroupId { get; set; }
        public  List<Group> StudentGroup { get; set; }
       public int DaysToPay { get; set; }
        public bool isFrozen { get; set; }
    }
}
