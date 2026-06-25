using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagement.Models.Domains
{
    // Event represents a community event
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public int OrganizerId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        // Junction relations:
        // 1. Many-to-many relationship with Participant via Registrations (junction with attributes)
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        // 2. Many-to-many relationship with Venue via EventVenue junction
        public ICollection<EventVenue> EventVenues { get; set; } = new List<EventVenue>();

        // 3. Many-to-many relationship with Activity via EventActivity junction
        public ICollection<EventActivity> EventActivities { get; set; } = new List<EventActivity>();
    }
}
