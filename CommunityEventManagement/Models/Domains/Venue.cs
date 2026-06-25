using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagement.Models.Domains
{
    // Venue details where events are hosted
    public class Venue
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Range(1, 10000, ErrorMessage = "Capacity must be between 1 and 10,000")]
        public int Capacity { get; set; }

        // Many-to-Many Navigation: Event can take place at various Venues
        public ICollection<EventVenue> EventVenues { get; set; } = new List<EventVenue>();
    }
}
