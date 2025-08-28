using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MaksGym.Models.ViewModels
{
    public class AddStudentAndGroupViewModel
    {
        [ValidateNever]
        public GroupViewModel GroupViewModel { get; set; } = new GroupViewModel();
        public StudentsInGroupViewModel StudentsInGroupViewModel { get; set;} = new StudentsInGroupViewModel();
    }
}
