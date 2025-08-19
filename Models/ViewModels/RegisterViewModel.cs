using System;
using System.ComponentModel.DataAnnotations;

namespace MaksGym.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Вкажіть номер телефону")]
        [Phone(ErrorMessage = "Некоректний формат номера телефону")]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Вкажіть повне ім'я")]
        [Display(Name = "ПІБ")]
        public string FullName { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Дата народження")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Вкажіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Підтвердіть пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
