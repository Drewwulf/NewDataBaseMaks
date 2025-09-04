namespace MaksGym.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public List<string> Groups { get; set; } = new();
        public List<string> Directions { get; set; } = new();
        public string? PhotoUrl { get; set; }
        public List<Group> groups { get; set; } = new List<Group>();
    }


}
