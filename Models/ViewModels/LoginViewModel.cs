using System.ComponentModel.DataAnnotations;

namespace MaksGym.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required, Phone]
        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = null!;

        [Display(Name = "Запам’ятати мене")]
        public bool RememberMe { get; set; }
    }
}
