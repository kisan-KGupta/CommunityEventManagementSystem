using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Models.Domains;

namespace CommunityEventManagement.Tests.MocksRepo
{
    public class MockEventRepository : IEventRepository
    {
        private readonly List<Event> _events = new();

        public int AddCallCount { get; private set; }
        public int UpdateCallCount { get; private set; }
        public int DeleteCallCount { get; private set; }
        public Event? LastAddedEvent { get; private set; }

        public void SeedEvents(params Event[] events)
        {
            _events.Clear();
            _events.AddRange(events);
        }

        public Task<List<Event>> GetAllAsync()
        {
            return Task.FromResult(_events.ToList());
        }

        public Task<Event?> GetByIdAsync(int id)
        {
            var target = _events.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(target);
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            var exists = _events.Any(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }

        public Task AddAsync(Event communityEvent, List<int> venueIds, List<int> activityIds)
        {
            AddCallCount++;
            communityEvent.Id = _events.Count + 1;
            
            // Map fake junctions
            foreach (var vId in venueIds)
            {
                communityEvent.EventVenues.Add(new EventVenue { EventId = communityEvent.Id, VenueId = vId });
            }
            foreach (var aId in activityIds)
            {
                communityEvent.EventActivities.Add(new EventActivity { EventId = communityEvent.Id, ActivityId = aId });
            }

            LastAddedEvent = communityEvent;
            _events.Add(communityEvent);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Event communityEvent, List<int> venueIds, List<int> activityIds)
        {
            UpdateCallCount++;
            var index = _events.FindIndex(e => e.Id == communityEvent.Id);
            if (index >= 0)
            {
                _events[index] = communityEvent;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            DeleteCallCount++;
            _events.RemoveAll(e => e.Id == id);
            return Task.CompletedTask;
        }
    }
}
