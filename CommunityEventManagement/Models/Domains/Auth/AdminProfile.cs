using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagement.Models.Domains.Auth
{
    // AdminProfile represents the system administrator's additional information
    public class AdminProfile : UserProfile
    {
        [Required]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty; // e.g., "Event Manager", "Coordinator"

        // OOP: Method Hiding
        public new string GetProfileDetails()
        {
            return $"Administrator Profile - {Position}";
        }
    }
}
