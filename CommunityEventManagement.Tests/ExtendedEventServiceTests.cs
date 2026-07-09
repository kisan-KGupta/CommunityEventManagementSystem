using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CommunityEventManagement.Services;
using CommunityEventManagement.Models.ViewModels;
using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Tests.MocksRepo;
using CommunityEventManagement.Exceptions;

namespace CommunityEventManagement.Tests
{
    public class ExtendedEventServiceTests
    {
        [Fact]
        public async Task GetAllAsync_Empty_ReturnsEmpty()
        {
            var repo = new MockEventRepository();
            var service = new EventService(repo);
            var result = await service.GetAllAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_Existing_ReturnsEvent()
        {
            var repo = new MockEventRepository();
            repo.SeedEvents(new Event { Id = 1, Name = "E1" });
            var service = new EventService(repo);
            var result = await service.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal("E1", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_NotExisting_ThrowsException()
        {
            var repo = new MockEventRepository();
            var service = new EventService(repo);
            await Assert.ThrowsAsync<EventNotFoundException>(() => service.GetByIdAsync(99));
        }

        [Fact]
        public async Task AddAsync_WithNullVenues_AddsEvent()
        {
            var repo = new MockEventRepository();
            var service = new EventService(repo);
            await service.AddAsync(new EventViewModel { Name = "E1" });
            Assert.Equal(1, repo.AddCallCount);
        }

        [Fact]
        public async Task UpdateAsync_Existing_UpdatesEvent()
        {
            var repo = new MockEventRepository();
            repo.SeedEvents(new Event { Id = 1, Name = "E1" });
            var service = new EventService(repo);
            await service.UpdateAsync(new EventViewModel { Id = 1, Name = "E1_Upd" });
            Assert.Equal(1, repo.UpdateCallCount);
        }

        [Fact]
        public async Task UpdateAsync_NotExisting_ThrowsException()
        {
            var repo = new MockEventRepository();
            var service = new EventService(repo);
            await Assert.ThrowsAsync<EventNotFoundException>(() => service.UpdateAsync(new EventViewModel { Id = 99 }));
        }

        [Fact]
        public async Task DeleteAsync_NotExisting_ThrowsException()
        {
            var repo = new MockEventRepository();
            var service = new EventService(repo);
            await Assert.ThrowsAsync<EventNotFoundException>(() => service.DeleteAsync(99));
        }

        [Fact]
        public async Task DeleteAsync_Existing_CallsDelete()
        {
            var repo = new MockEventRepository();
            repo.SeedEvents(new Event { Id = 2, Name = "E2" });
            var service = new EventService(repo);
            await service.DeleteAsync(2);
            Assert.Equal(1, repo.DeleteCallCount);
        }

        [Fact]
        public async Task AddAsync_SetsDateProperly()
        {
            var repo = new MockEventRepository();
            var service = new EventService(repo);
            var dt = DateTime.Now.AddDays(1);
            await service.AddAsync(new EventViewModel { Name = "E1", Date = dt });
            Assert.Equal(dt, repo.LastAddedEvent?.Date);
        }

        [Fact]
        public async Task AddAsync_MultipleEvents_CorrectCounts()
        {
            var repo = new MockEventRepository();
            var service = new EventService(repo);
            await service.AddAsync(new EventViewModel { Name = "E1" });
            await service.AddAsync(new EventViewModel { Name = "E2" });
            Assert.Equal(2, repo.AddCallCount);
        }
    }
}
