using System.ComponentModel.DataAnnotations;

namespace MaksGym.Models.ViewModels
{
    public class GroupViewModel
    {
        public List<Group> Groups { get; set; } = new List<Group>();
        public List<Direction> Directions { get; set; } = new List<Direction>();
        public List<Coach> Coaches { get; set; } = new List<Coach>();

        [Required(ErrorMessage ="Вкажіть назву групи")]
        public string GroupName { get; set; } 
        [Required(ErrorMessage ="Виберіть тренера")]
        public int CoachId { get; set; }

        [Required(ErrorMessage ="Виберіть напрямок групи")]
        public int DirectionId{ get; set; }
        [Required(ErrorMessage = "Вкажіть опис групи")]
        public string GroupDescription { get; set; } = null!;

        public int GroupId { get; set; }

    }
}
