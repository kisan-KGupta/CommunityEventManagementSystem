using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityEventManagement.Models.Domains.Auth;

namespace CommunityEventManagement.Models.Domains
{
    // Junction table recording registrations (Participant <-> Event)
    public class Registration
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Participant))]
        public int ParticipantId { get; set; }
        public Participant Participant { get; set; } = null!;

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // e.g., "Pending", "Confirmed", "Cancelled"
    }

    // Junction table for Event <-> Venue many-to-many
    public class EventVenue
    {
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public int VenueId { get; set; }
        public Venue Venue { get; set; } = null!;
    }

    // Junction table for Event <-> Activity many-to-many
    public class EventActivity
    {
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public int ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;
    }
}
