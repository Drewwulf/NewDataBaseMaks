namespace MaksGym.Models.ViewModels
{
    public class CoachViewModel
    {
        public Coach NewCoach { get; set; } = new Coach();
        public List<Coach> CoachList { get; set; } = new List<Coach>();

        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();


    }
}
