using System;
using System.ComponentModel.DataAnnotations;

namespace MaksGym.Models
{
    public class UserDetailViewModel
    {
        public ApplicationUser User { get; set; } = new();

        public class ApplicationUser
        {
            [Required(ErrorMessage = "Вкажіть повне ім'я")]
            [Display(Name = "ПІБ")]
            public string FullName { get; set; } = null!;

            [Required(ErrorMessage = "Вкажіть номер телефону")]
            [Phone(ErrorMessage = "Некоректний формат номера телефону")]
            [Display(Name = "Номер телефону")]
            public string PhoneNumber { get; set; } = null!;

            [DataType(DataType.Date)]
            [Display(Name = "Дата народження")]
            public DateTime? DateOfBirth { get; set; }

            [Display(Name = "Роль")]
            public string Role { get; set; } = "Student"; 

            [Display(Name = "Активний")]
            public bool IsActive { get; set; } = true;

            public string Id { get; set; } = null!;
        }
    }
}
