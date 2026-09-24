using JobBoard.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace JobBoard.Domain.Entities
{
    
    public class ApplicationUser : IdentityUser
    {
        public UserType User_Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public RecruiterProfile RecruiterProfile { get; set; }
        public CandidateProfile CandidateProfile { get; set; }
        public List<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
