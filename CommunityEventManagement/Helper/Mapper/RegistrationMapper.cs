using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Models.ViewModels;

namespace CommunityEventManagement.Helper.Mapper
{
    public static class RegistrationMapper
    {
        public static RegistrationViewModel ToViewModel(Registration domain)
        {
            return new RegistrationViewModel
            {
                Id = domain.Id,
                EventId = domain.EventId,
                EventName = domain.Event?.Name ?? string.Empty,
                EventDate = domain.Event?.Date ?? System.DateTime.MinValue,
                ParticipantId = domain.ParticipantId,
                ParticipantName = domain.Participant?.User?.FullName ?? string.Empty,
                ParticipantEmail = domain.Participant?.User?.Email ?? string.Empty,
                RegistrationDate = domain.RegistrationDate,
                Status = domain.Status
            };
        }
    }
}
