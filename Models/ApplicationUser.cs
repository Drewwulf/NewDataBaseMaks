using Microsoft.AspNetCore.Identity;

namespace MaksGym.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;      // додаткове поле
        public DateTime? DateOfBirth { get; set; }         // додаткове поле
        public string? PhotoUrl { get; set; }               // додаткове поле
        // Номер телефону НЕ додаємо, бо вже є в IdentityUser.PhoneNumber
        // Інші базові поля (Email, UserName, PasswordHash і т.д.) теж не додаємо
    }
}
