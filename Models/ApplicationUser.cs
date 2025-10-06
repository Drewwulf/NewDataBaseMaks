using Microsoft.AspNetCore.Identity;

namespace MaksGym.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = null!;    
        public DateTime? DateOfBirth { get; set; }        
        public string? PhotoUrl { get; set; }        
        

    }
}
