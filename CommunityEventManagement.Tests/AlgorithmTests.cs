using System.Threading.Tasks;
using Xunit;
using CommunityEventManagement.Services;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Models.Domains;
using System.Collections.Generic;
using System;

namespace CommunityEventManagement.Tests
{
    public class AlgorithmTests
    {
        private class FakeEventRepo : IEventRepository
        {
            public Task AddAsync(Event ev, List<int> venueIds, List<int> activityIds) => Task.CompletedTask;
            public Task DeleteAsync(int id) => Task.CompletedTask;
            public Task<bool> ExistsByNameAsync(string name) => Task.FromResult(false);
            public Task UpdateAsync(Event ev, List<int> venueIds, List<int> activityIds) => Task.CompletedTask;
            public Task<Event> GetByIdAsync(int id) => Task.FromResult(new Event());
            
            public Task<List<Event>> GetAllAsync()
            {
                var events = new List<Event>
                {
                    new Event { Id = 1, Name = "Zeta Event", StartDate = new DateTime(2025, 5, 1) },
                    new Event { Id = 2, Name = "Alpha Event", StartDate = new DateTime(2025, 1, 1) },
                    new Event { Id = 3, Name = "Beta Event", StartDate = new DateTime(2025, 3, 1) }
                };
                return Task.FromResult(events);
            }
        }

        [Fact]
        public async Task GetAllAsync_DateSort_BubbleSortProducesCorrectOrder()
        {
            var service = new EventService(new FakeEventRepo());
            var sorted = await service.GetAllAsync("Date");

            Assert.Equal("Alpha Event", sorted[0].Name);
            Assert.Equal("Beta Event", sorted[1].Name);
            Assert.Equal("Zeta Event", sorted[2].Name);
        }

        [Fact]
        public async Task FindEventByNameAsync_BinarySearch_FindsCorrectEvent()
        {
            var service = new EventService(new FakeEventRepo());
            var ev = await service.FindEventByNameAsync("Beta Event");

            Assert.NotNull(ev);
            Assert.Equal(3, ev.Id);
        }
    }
}
