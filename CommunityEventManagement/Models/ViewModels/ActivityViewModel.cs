using System.ComponentModel.DataAnnotations;

namespace CommunityEventManagement.Models.ViewModels
{
    public class ActivityViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Activity name is required")]
        [StringLength(100, ErrorMessage = "Activity name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Activity type is required")]
        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters")]
        public string Type { get; set; } = string.Empty; // e.g., "Workshop", "Talk", "Game"

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;
    }
}
