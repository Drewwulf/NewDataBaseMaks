using System.ComponentModel.DataAnnotations;

namespace MaksGym.Models.ViewModels
{
    public class StudentsInGroupViewModel
    {
        [Required(ErrorMessage = "Вкажіть користувача")]
        public int? UserId { get; set; }
        [Required(ErrorMessage = "Вкажіть групу")]
        public int GroupId { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();
        public List<StudentToGroup> StudentInGroup { get; set; } = new List<StudentToGroup>();
        public Group? NewGroup { get; set; } = null!;
    }
}
