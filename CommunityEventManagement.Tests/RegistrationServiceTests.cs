using System.Threading.Tasks;
using Xunit;
using CommunityEventManagement.Services;
using CommunityEventManagement.Exceptions;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Models.Domains;
using System.Collections.Generic;

namespace CommunityEventManagement.Tests
{
    public class RegistrationServiceTests
    {
        private class FakeRegistrationRepo : IRegistrationRepository
        {
            public bool IsDuplicate { get; set; }
            public bool IsFull { get; set; }

            public Task AddAsync(Registration registration) => Task.CompletedTask;
            public Task<Registration> GetByIdAsync(int id) => Task.FromResult(new Registration());
            public Task<List<Registration>> GetAllAsync() => Task.FromResult(new List<Registration>());
            public Task UpdateAsync(Registration registration) => Task.CompletedTask;
            public Task UpdateStatusAsync(int id, string status) => Task.CompletedTask;
            public Task DeleteAsync(int id) => Task.CompletedTask;
            public Task<List<Registration>> GetByParticipantIdAsync(int participantId) => Task.FromResult(new List<Registration>());
            
            public Task<bool> ExistsAsync(int participantId, int eventId)
            {
                return Task.FromResult(IsDuplicate);
            }

            public Task<int> GetRegistrationCountForEventAsync(int eventId)
            {
                return Task.FromResult(IsFull ? 1000 : 0); // Assuming 1000 is over capacity
            }
        }

        private class FakeEventRepo : IEventRepository
        {
            public Task AddAsync(Event ev, List<int> venueIds, List<int> activityIds) => Task.CompletedTask;
            public Task DeleteAsync(int id) => Task.CompletedTask;
            public Task<bool> ExistsByNameAsync(string name) => Task.FromResult(false);
            public Task<List<Event>> GetAllAsync() => Task.FromResult(new List<Event>());
            public Task UpdateAsync(Event ev, List<int> venueIds, List<int> activityIds) => Task.CompletedTask;
            
            public Task<Event> GetByIdAsync(int id)
            {
                var ev = new Event 
                { 
                    Id = id, 
                    Name = "Test Event",
                    EventVenues = new List<EventVenue> { new EventVenue { Venue = new Venue { Capacity = 2 } } },
                    Registrations = new List<Registration> 
                    { 
                        new Registration { Status = "Confirmed" },
                        new Registration { Status = "Confirmed" }
                    }
                };
                return Task.FromResult(ev);
            }
        }

        [Fact]
        public async Task RegisterParticipantAsync_DuplicateRegistration_ThrowsDuplicateRegistrationException()
        {
            var fakeRegRepo = new FakeRegistrationRepo { IsDuplicate = true };
            var fakeEventRepo = new FakeEventRepo();
            var service = new RegistrationService(fakeRegRepo, fakeEventRepo);

            await Assert.ThrowsAsync<DuplicateRegistrationException>(() => service.RegisterParticipantAsync(1, 1));
        }

        [Fact]
        public async Task RegisterParticipantAsync_EventFull_ThrowsEventFullException()
        {
            var fakeRegRepo = new FakeRegistrationRepo { IsDuplicate = false, IsFull = true };
            var fakeEventRepo = new FakeEventRepo();
            var service = new RegistrationService(fakeRegRepo, fakeEventRepo);

            await Assert.ThrowsAsync<EventFullException>(() => service.RegisterParticipantAsync(1, 1));
        }
    }
}
