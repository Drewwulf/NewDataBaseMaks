namespace MaksGym.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public List<string> Groups { get; set; } = new();
        public List<string> Directions { get; set; } = new();
        public string? PhotoUrl { get; set; }
        public List<Group> groups { get; set; } = new List<Group>();
        public List<Subscription> subscriptions { get; set; } = new List<Subscription>();
        public List<StudentsToSubscription> studentsToSubscriptions { get; set; } = new List<StudentsToSubscription>();

        public List<Transaction>? transactions { get; set; }
        public List<Training>? trainings { get; set; }

    }
}
