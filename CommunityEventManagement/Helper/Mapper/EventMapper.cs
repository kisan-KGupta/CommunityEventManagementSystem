using System.Linq;
using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Models.ViewModels;

namespace CommunityEventManagement.Helper.Mapper
{
    public static class EventMapper
    {
        public static EventViewModel ToViewModel(Event domain)
        {
            return new EventViewModel
            {
                Id = domain.Id,
                Name = domain.Name,
                Date = domain.Date,
                StartDate = domain.StartDate,
                EndDate = domain.EndDate,
                OrganizerId = domain.OrganizerId,
                Description = domain.Description,
                SelectedVenueIds = domain.EventVenues.Select(ev => ev.VenueId).ToList(),
                VenueNamesDisplay = string.Join(", ", domain.EventVenues.Select(ev => ev.Venue?.Name).Where(n => n != null)),
                SelectedActivityIds = domain.EventActivities.Select(ea => ea.ActivityId).ToList(),
                ActivityNamesDisplay = string.Join(", ", domain.EventActivities.Select(ea => ea.Activity?.Name).Where(n => n != null)),
                ParticipantCount = domain.Registrations.Count
            };
        }

        public static Event ToDomainModel(EventViewModel model)
        {
            return new Event
            {
                Id = model.Id,
                Name = model.Name,
                Date = model.Date,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                OrganizerId = model.OrganizerId,
                Description = model.Description
            };
        }

        public static void UpdateDomainModel(Event domain, EventViewModel model)
        {
            domain.Name = model.Name;
            domain.Date = model.Date;
            domain.Description = model.Description;
        }
    }
}
