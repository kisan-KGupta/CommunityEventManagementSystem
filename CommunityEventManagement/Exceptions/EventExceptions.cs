using System;

namespace CommunityEventManagement.Exceptions
{
    // Thrown when an administrator attempts to create an event with a name that already exists
    public class DuplicateEventException : Exception
    {
        public DuplicateEventException(string name) 
            : base($"An event with the name '{name}' already exists.")
        {
        }
    }

    // Thrown when a queried event does not exist
    public class EventNotFoundException : Exception
    {
        public EventNotFoundException(int id) 
            : base($"Event with ID {id} was not found.")
        {
        }
    }

    // Thrown when a participant tries to register for an event that has reached maximum venue capacity
    public class EventFullException : Exception
    {
        public EventFullException(string eventName) 
            : base($"Registration failed. The event '{eventName}' is at full venue capacity.")
        {
        }
    }

    // Thrown when a participant attempts to register for the same event twice
    public class DuplicateRegistrationException : Exception
    {
        public DuplicateRegistrationException() 
            : base("You are already registered for this event.")
        {
        }
    }

    // Thrown when a queried venue does not exist
    public class VenueNotFoundException : Exception
    {
        public VenueNotFoundException(int id) 
            : base($"Venue with ID {id} was not found.")
        {
        }
    }

    // Thrown when a queried activity does not exist
    public class ActivityNotFoundException : Exception
    {
        public ActivityNotFoundException(int id) 
            : base($"Activity with ID {id} was not found.")
        {
        }
    }
}
