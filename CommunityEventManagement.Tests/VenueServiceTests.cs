using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using CommunityEventManagement.Services;
using CommunityEventManagement.Models.ViewModels;
using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Tests.MocksRepo;
using CommunityEventManagement.Exceptions;

namespace CommunityEventManagement.Tests
{
    public class VenueServiceTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllVenues()
        {
            var repo = new MockVenueRepository();
            repo.SeedVenues(new Venue { Id = 1, Name = "V1" }, new Venue { Id = 2, Name = "V2" });
            var service = new VenueService(repo);
            var result = await service.GetAllAsync();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllAsync_Empty_ReturnsEmpty()
        {
            var repo = new MockVenueRepository();
            var service = new VenueService(repo);
            var result = await service.GetAllAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_Existing_ReturnsVenue()
        {
            var repo = new MockVenueRepository();
            repo.SeedVenues(new Venue { Id = 1, Name = "V1", Address = "Loc1" });
            var service = new VenueService(repo);
            var result = await service.GetByIdAsync(1);
            Assert.Equal("V1", result.Name);
            Assert.Equal("Loc1", result.Address);
        }

        [Fact]
        public async Task GetByIdAsync_NotExisting_ThrowsException()
        {
            var repo = new MockVenueRepository();
            var service = new VenueService(repo);
            await Assert.ThrowsAsync<VenueNotFoundException>(() => service.GetByIdAsync(99));
        }

        [Fact]
        public async Task AddAsync_New_AddsVenue()
        {
            var repo = new MockVenueRepository();
            var service = new VenueService(repo);
            await service.AddAsync(new VenueViewModel { Name = "V1", Address = "Loc1" });
            Assert.Equal(1, repo.AddCallCount);
            Assert.Equal("V1", repo.LastAddedVenue?.Name);
        }

        [Fact]
        public async Task AddAsync_DuplicateName_ThrowsException()
        {
            var repo = new MockVenueRepository();
            repo.SeedVenues(new Venue { Id = 1, Name = "V1" });
            var service = new VenueService(repo);
            var ex = await Assert.ThrowsAsync<Exception>(() => service.AddAsync(new VenueViewModel { Name = "V1" }));
            Assert.Contains("already exists", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_Existing_UpdatesVenue()
        {
            var repo = new MockVenueRepository();
            repo.SeedVenues(new Venue { Id = 1, Name = "V1" });
            var service = new VenueService(repo);
            await service.UpdateAsync(new VenueViewModel { Id = 1, Name = "V1_Updated" });
            Assert.Equal(1, repo.UpdateCallCount);
            var updated = await repo.GetByIdAsync(1);
            Assert.Equal("V1_Updated", updated?.Name);
        }

        [Fact]
        public async Task UpdateAsync_NotExisting_ThrowsException()
        {
            var repo = new MockVenueRepository();
            var service = new VenueService(repo);
            await Assert.ThrowsAsync<VenueNotFoundException>(() => service.UpdateAsync(new VenueViewModel { Id = 99, Name = "X" }));
        }

        [Fact]
        public async Task DeleteAsync_Existing_DeletesVenue()
        {
            var repo = new MockVenueRepository();
            repo.SeedVenues(new Venue { Id = 1, Name = "V1" });
            var service = new VenueService(repo);
            await service.DeleteAsync(1);
            Assert.Equal(1, repo.DeleteCallCount);
            Assert.Null(await repo.GetByIdAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_NotExisting_ThrowsException()
        {
            var repo = new MockVenueRepository();
            var service = new VenueService(repo);
            await Assert.ThrowsAsync<VenueNotFoundException>(() => service.DeleteAsync(99));
        }

        [Fact]
        public async Task AddAsync_Multiple_IncrementsCount()
        {
            var repo = new MockVenueRepository();
            var service = new VenueService(repo);
            await service.AddAsync(new VenueViewModel { Name = "V1" });
            await service.AddAsync(new VenueViewModel { Name = "V2" });
            Assert.Equal(2, repo.AddCallCount);
        }

        [Fact]
        public async Task GetAllAsync_AfterAdd_ReturnsAll()
        {
            var repo = new MockVenueRepository();
            var service = new VenueService(repo);
            await service.AddAsync(new VenueViewModel { Name = "V1" });
            var all = await service.GetAllAsync();
            Assert.Single(all);
        }

        [Fact]
        public async Task DeleteAsync_Multiple_CorrectlyDeletes()
        {
            var repo = new MockVenueRepository();
            repo.SeedVenues(new Venue { Id = 1 }, new Venue { Id = 2 });
            var service = new VenueService(repo);
            await service.DeleteAsync(1);
            await service.DeleteAsync(2);
            var all = await service.GetAllAsync();
            Assert.Empty(all);
        }
        
        [Fact]
        public async Task GetByIdAsync_DifferentIds_ReturnsCorrect()
        {
            var repo = new MockVenueRepository();
            repo.SeedVenues(new Venue { Id = 1, Name = "V1" }, new Venue { Id = 2, Name = "V2" });
            var service = new VenueService(repo);
            var r1 = await service.GetByIdAsync(1);
            var r2 = await service.GetByIdAsync(2);
            Assert.Equal("V1", r1.Name);
            Assert.Equal("V2", r2.Name);
        }

        [Fact]
        public async Task UpdateAsync_MaintainsId()
        {
            var repo = new MockVenueRepository();
            repo.SeedVenues(new Venue { Id = 5, Name = "V5" });
            var service = new VenueService(repo);
            await service.UpdateAsync(new VenueViewModel { Id = 5, Name = "NewName" });
            var item = await repo.GetByIdAsync(5);
            Assert.Equal(5, item?.Id);
            Assert.Equal("NewName", item?.Name);
        }
    }
}
