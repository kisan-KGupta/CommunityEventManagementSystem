using System;
using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Models.ViewModels;

namespace CommunityEventManagement.Services
{
    // Design Pattern: Creational - Factory Pattern
    // Encapsulates the logic of instantiating complex Event objects based on input parameters.
    public class EventFactory
    {
        public static Event CreateEvent(EventViewModel model, string eventType)
        {
            var newEvent = new Event
            {
                Name = model.Name,
                Description = model.Description,
                Date = model.StartDate,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                OrganizerId = model.OrganizerId
            };

            // Business logic applied based on event type
            if (eventType.Equals("VIP", StringComparison.OrdinalIgnoreCase))
            {
                newEvent.Name = "[VIP] " + newEvent.Name;
                // Assign special flags or properties
            }
            else if (eventType.Equals("Public", StringComparison.OrdinalIgnoreCase))
            {
                newEvent.Name = "[Public] " + newEvent.Name;
            }

            return newEvent;
        }
    }
}
