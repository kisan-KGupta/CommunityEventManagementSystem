using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagement.Models.Domains
{
    // Activity details that can belong to events
    public class Activity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // e.g., "Workshop", "Talk", "Game"

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        // Many-to-Many Navigation: Activity can be part of multiple Events
        public ICollection<EventActivity> EventActivities { get; set; } = new List<EventActivity>();
    }
}
