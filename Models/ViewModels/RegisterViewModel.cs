using System.ComponentModel.DataAnnotations;

namespace MaksGym.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string FullName { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
