using System;

namespace CommunityEventManagement.Models.ViewModels
{
    public class RegistrationViewModel
    {
        public int Id { get; set; }
        
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }

        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public string ParticipantEmail { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
