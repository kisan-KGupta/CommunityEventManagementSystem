using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Models.Domains;

namespace CommunityEventManagement.Tests.MocksRepo
{
    public class MockVenueRepository : IVenueRepository
    {
        private readonly List<Venue> _venues = new();

        public int AddCallCount { get; private set; }
        public int UpdateCallCount { get; private set; }
        public int DeleteCallCount { get; private set; }
        public Venue? LastAddedVenue { get; private set; }

        public void SeedVenues(params Venue[] venues)
        {
            _venues.Clear();
            _venues.AddRange(venues);
        }

        public Task<List<Venue>> GetAllAsync()
        {
            return Task.FromResult(_venues.ToList());
        }

        public Task<Venue?> GetByIdAsync(int id)
        {
            var target = _venues.FirstOrDefault(v => v.Id == id);
            return Task.FromResult(target);
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            var exists = _venues.Any(v => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }

        public Task AddAsync(Venue venue)
        {
            AddCallCount++;
            venue.Id = _venues.Count > 0 ? _venues.Max(v => v.Id) + 1 : 1;
            LastAddedVenue = venue;
            _venues.Add(venue);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Venue venue)
        {
            UpdateCallCount++;
            var index = _venues.FindIndex(v => v.Id == venue.Id);
            if (index >= 0)
            {
                _venues[index] = venue;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            DeleteCallCount++;
            _venues.RemoveAll(v => v.Id == id);
            return Task.CompletedTask;
        }
    }
}
