namespace MaksGym.Models.ViewModels
{
    public class StudentViewModel
    {

        public Student NewStudent { get; set; } = new Student();


        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();


        public List<Student> students { get; set; } = new List<Student>();
        public List<Student> StudentsInGroup { get; set; } = new List<Student>();
    }
}
