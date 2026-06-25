using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CommunityEventManagement.Exceptions;
using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Models.ViewModels;
using CommunityEventManagement.Services;
using CommunityEventManagement.Tests.MocksRepo;

namespace CommunityEventManagement.Tests
{
    public class EventServiceTests
    {
        [Fact]
        public async Task GetAllAsync_WhenEventsExist_ShouldMapToViewModels()
        {
            // Arrange
            var repo = new MockEventRepository();
            repo.SeedEvents(
                new Event { Id = 1, Name = "Charity Run", Description = "Running for a cause", Date = DateTime.Now.AddDays(5) },
                new Event { Id = 2, Name = "Coding Camp", Description = "Hackathon and learning sessions", Date = DateTime.Now.AddDays(10) }
            );
            var service = new EventService(repo);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].Id);
            Assert.Equal("Charity Run", result[0].Name);
            Assert.Equal("Coding Camp", result[1].Name);
        }

        [Fact]
        public async Task AddAsync_WhenEventNameDoesNotExist_ShouldAddEvent()
        {
            // Arrange
            var repo = new MockEventRepository();
            var service = new EventService(repo);
            var model = new EventViewModel
            {
                Name = "Winter Gala",
                Description = "Annual end of year celebration",
                Date = DateTime.Now.AddDays(30),
                SelectedVenueIds = new List<int> { 1 },
                SelectedActivityIds = new List<int> { 2 }
            };

            // Act
            await service.AddAsync(model);

            // Assert
            Assert.Equal(1, repo.AddCallCount);
            Assert.NotNull(repo.LastAddedEvent);
            Assert.Equal("Winter Gala", repo.LastAddedEvent.Name);
            Assert.Contains(1, model.SelectedVenueIds);
        }

        [Fact]
        public async Task AddAsync_WhenEventNameAlreadyExists_ShouldThrowDuplicateEventException()
        {
            // Arrange
            var repo = new MockEventRepository();
            repo.SeedEvents(new Event { Id = 1, Name = "Winter Gala", Description = "Existing celebration" });
            var service = new EventService(repo);
            var model = new EventViewModel
            {
                Name = "Winter Gala",
                Description = "New celebration",
                Date = DateTime.Now.AddDays(30)
            };

            // Act & Assert
            await Assert.ThrowsAsync<DuplicateEventException>(() => service.AddAsync(model));
            Assert.Equal(0, repo.AddCallCount); // Verify repository was not called
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEvent()
        {
            // Arrange
            var repo = new MockEventRepository();
            repo.SeedEvents(new Event { Id = 5, Name = "Art Exhibition", Description = "Local community paintings" });
            var service = new EventService(repo);

            // Act
            await service.DeleteAsync(5);

            // Assert
            Assert.Equal(1, repo.DeleteCallCount);
            Assert.Null(await repo.GetByIdAsync(5));
        }
    }
}
