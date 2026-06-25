using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagement.Models.ViewModels
{
    public class VenueViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Venue name is required")]
        [StringLength(100, ErrorMessage = "Venue name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Venue address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 10000, ErrorMessage = "Capacity must be between 1 and 10,000")]
        public int Capacity { get; set; }
    }
}
