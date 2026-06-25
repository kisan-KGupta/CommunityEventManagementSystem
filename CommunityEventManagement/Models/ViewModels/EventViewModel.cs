using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagement.Models.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date and time is required")]
        public DateTime Date { get; set; } = DateTime.Now.AddDays(1); // Default to tomorrow

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(1);

        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1).AddHours(2);

        public int OrganizerId { get; set; }

        [Required(ErrorMessage = "Event description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        // IDs of Venues selected for this event
        public List<int> SelectedVenueIds { get; set; } = new();

        // Names of Venues (for display on UI)
        public string VenueNamesDisplay { get; set; } = string.Empty;

        // IDs of Activities selected for this event
        public List<int> SelectedActivityIds { get; set; } = new();

        // Names of Activities (for display on UI)
        public string ActivityNamesDisplay { get; set; } = string.Empty;

        // Count of registered participants
        public int ParticipantCount { get; set; }
    }
}
