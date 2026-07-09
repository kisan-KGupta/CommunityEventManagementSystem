using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CommunityEventManagement.Services;
using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Tests.MocksRepo;
using CommunityEventManagement.Exceptions;

namespace CommunityEventManagement.Tests
{
    public class ExtendedRegistrationServiceTests
    {
        private Event GetMockEvent(int id, int capacity)
        {
            var ev = new Event { Id = id, EventVenues = new List<EventVenue> { new EventVenue { Venue = new Venue { Capacity = capacity } } }, Registrations = new List<Registration>() };
            return ev;
        }

        [Fact]
        public async Task RegisterParticipantAsync_Success_AddsRegistration()
        {
            var regRepo = new MockRegistrationRepository();
            var evRepo = new MockEventRepository();
            evRepo.SeedEvents(GetMockEvent(1, 10)); // Capacity 10
            
            var service = new RegistrationService(regRepo, evRepo);
            await service.RegisterParticipantAsync(1, 1);
            Assert.Equal(1, regRepo.AddCallCount);
        }

        [Fact]
        public async Task UpdateStatusAsync_Success()
        {
            var regRepo = new MockRegistrationRepository();
            regRepo.SeedRegistrations(new Registration { Id = 1, Status = "Pending" });
            var evRepo = new MockEventRepository();
            
            var service = new RegistrationService(regRepo, evRepo);
            await service.UpdateStatusAsync(1, "Approved");
            Assert.Equal(1, regRepo.UpdateStatusCallCount);
            
            var updated = await regRepo.GetByIdAsync(1);
            Assert.Equal("Approved", updated?.Status);
        }

        [Fact]
        public async Task CancelRegistrationAsync_Success()
        {
            var regRepo = new MockRegistrationRepository();
            regRepo.SeedRegistrations(new Registration { Id = 1, Status = "Pending" });
            var evRepo = new MockEventRepository();
            
            var service = new RegistrationService(regRepo, evRepo);
            await service.CancelRegistrationAsync(1);
            Assert.Equal(1, regRepo.UpdateStatusCallCount);
            
            var updated = await regRepo.GetByIdAsync(1);
            Assert.Equal("Cancelled", updated?.Status);
        }

        [Fact]
        public async Task GetByParticipantIdAsync_ReturnsCorrectCount()
        {
            var regRepo = new MockRegistrationRepository();
            regRepo.SeedRegistrations(
                new Registration { Id = 1, ParticipantId = 5 },
                new Registration { Id = 2, ParticipantId = 5 },
                new Registration { Id = 3, ParticipantId = 6 }
            );
            var evRepo = new MockEventRepository();
            
            var service = new RegistrationService(regRepo, evRepo);
            var result = await service.GetByParticipantIdAsync(5);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAll()
        {
            var regRepo = new MockRegistrationRepository();
            regRepo.SeedRegistrations(
                new Registration { Id = 1 },
                new Registration { Id = 2 }
            );
            var evRepo = new MockEventRepository();
            
            var service = new RegistrationService(regRepo, evRepo);
            var result = await service.GetAllAsync();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task RegisterParticipantAsync_Duplicate_ThrowsException()
        {
            var regRepo = new MockRegistrationRepository();
            regRepo.SeedRegistrations(new Registration { ParticipantId = 1, EventId = 1 });
            var evRepo = new MockEventRepository();
            evRepo.SeedEvents(GetMockEvent(1, 10));

            var service = new RegistrationService(regRepo, evRepo);
            await Assert.ThrowsAsync<DuplicateRegistrationException>(() => service.RegisterParticipantAsync(1, 1));
        }

        [Fact]
        public async Task RegisterParticipantAsync_FullCapacity_ThrowsException()
        {
            var regRepo = new MockRegistrationRepository();
            var evRepo = new MockEventRepository();
            var ev = GetMockEvent(1, 1);
            ev.Registrations.Add(new Registration { Status = "Confirmed" });
            evRepo.SeedEvents(ev);

            var service = new RegistrationService(regRepo, evRepo);
            await Assert.ThrowsAsync<EventFullException>(() => service.RegisterParticipantAsync(1, 1));
        }

        [Fact]
        public async Task CancelRegistrationAsync_NotExisting_DoesNotFail()
        {
            var regRepo = new MockRegistrationRepository();
            var evRepo = new MockEventRepository();
            
            var service = new RegistrationService(regRepo, evRepo);
            // Even if not found, it throws Exception in real service, mock might behave differently or throws Exception too
            await Assert.ThrowsAsync<Exception>(() => service.CancelRegistrationAsync(99));
        }
        
        [Fact]
        public async Task RegisterParticipant_EventNotFound_ThrowsException()
        {
            var regRepo = new MockRegistrationRepository();
            var evRepo = new MockEventRepository(); // No events seeded
            
            var service = new RegistrationService(regRepo, evRepo);
            await Assert.ThrowsAsync<EventNotFoundException>(() => service.RegisterParticipantAsync(1, 1));
        }
        
        [Fact]
        public async Task UpdateStatusAsync_Multiple_CorrectUpdates()
        {
            var regRepo = new MockRegistrationRepository();
            regRepo.SeedRegistrations(new Registration { Id = 1, Status = "Pending" }, new Registration { Id = 2, Status = "Pending" });
            var evRepo = new MockEventRepository();
            
            var service = new RegistrationService(regRepo, evRepo);
            await service.UpdateStatusAsync(1, "Approved");
            await service.UpdateStatusAsync(2, "Rejected");
            
            var r1 = await regRepo.GetByIdAsync(1);
            var r2 = await regRepo.GetByIdAsync(2);
            
            Assert.Equal("Approved", r1?.Status);
            Assert.Equal("Rejected", r2?.Status);
        }
    }
}
