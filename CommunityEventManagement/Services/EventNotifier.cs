using System;
using System.Collections.Generic;
using CommunityEventManagement.Models.Domains;

namespace CommunityEventManagement.Services
{
    // Design Pattern: Behavioral - Observer Pattern
    // Defines a subscription mechanism to notify multiple objects about any events that happen to the object they're observing.

    // The Observer Interface
    public interface IEventObserver
    {
        void Update(string message);
    }

    // The Subject Interface
    public interface IEventSubject
    {
        void Attach(IEventObserver observer);
        void Detach(IEventObserver observer);
        void Notify(string message);
    }

    // Concrete Subject
    public class EventNotifier : IEventSubject
    {
        private readonly List<IEventObserver> _observers = new List<IEventObserver>();

        public void Attach(IEventObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IEventObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }

        public void TriggerEventChange(Event ev, string status)
        {
            Notify($"Event '{ev.Name}' status has changed to: {status} at {DateTime.UtcNow}");
        }
    }

    // Concrete Observer
    public class ParticipantObserver : IEventObserver
    {
        private readonly string _participantName;

        public ParticipantObserver(string participantName)
        {
            _participantName = participantName;
        }

        public void Update(string message)
        {
            // In a real application, this would send an email or push notification
            Console.WriteLine($"[Notification sent to {_participantName}]: {message}");
        }
    }
}
