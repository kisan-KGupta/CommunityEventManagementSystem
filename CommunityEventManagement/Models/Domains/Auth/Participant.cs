using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagement.Models.Domains.Auth
{
    // Participant represents details about a user registered for community events
    public class Participant : UserProfile
    {
        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        // Navigation Property: A participant can register for multiple events
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        // OOP: Method Overriding (Overriding the base method)
        public override string GetProfileDetails()
        {
            return $"Participant Profile - {PhoneNumber}";
        }
    }
}
