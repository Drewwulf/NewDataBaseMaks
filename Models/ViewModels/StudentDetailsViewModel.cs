namespace MaksGym.Models.ViewModels
{
    public class StudentDetailsViewModel
    {
        public Student NewStudent { get; set; }
       public List<Subscription> Subscriptions { get; set; }
        public int SubscriptionId { get; set; }
    }
}
