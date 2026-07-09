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
    public class ActivityServiceTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsAllActivities()
        {
            var repo = new MockActivityRepository();
            repo.SeedActivities(new Activity { Id = 1, Name = "A1" }, new Activity { Id = 2, Name = "A2" });
            var service = new ActivityService(repo);
            var result = await service.GetAllAsync();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllAsync_Empty_ReturnsEmpty()
        {
            var repo = new MockActivityRepository();
            var service = new ActivityService(repo);
            var result = await service.GetAllAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_Existing_ReturnsActivity()
        {
            var repo = new MockActivityRepository();
            repo.SeedActivities(new Activity { Id = 1, Name = "A1", Description = "Desc1" });
            var service = new ActivityService(repo);
            var result = await service.GetByIdAsync(1);
            Assert.Equal("A1", result.Name);
            Assert.Equal("Desc1", result.Description);
        }

        [Fact]
        public async Task GetByIdAsync_NotExisting_ThrowsException()
        {
            var repo = new MockActivityRepository();
            var service = new ActivityService(repo);
            await Assert.ThrowsAsync<ActivityNotFoundException>(() => service.GetByIdAsync(99));
        }

        [Fact]
        public async Task AddAsync_New_AddsActivity()
        {
            var repo = new MockActivityRepository();
            var service = new ActivityService(repo);
            await service.AddAsync(new ActivityViewModel { Name = "A1", Description = "Desc1" });
            Assert.Equal(1, repo.AddCallCount);
            Assert.Equal("A1", repo.LastAddedActivity?.Name);
        }

        [Fact]
        public async Task AddAsync_DuplicateName_ThrowsException()
        {
            var repo = new MockActivityRepository();
            repo.SeedActivities(new Activity { Id = 1, Name = "A1" });
            var service = new ActivityService(repo);
            var ex = await Assert.ThrowsAsync<Exception>(() => service.AddAsync(new ActivityViewModel { Name = "A1" }));
            Assert.Contains("already exists", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_Existing_UpdatesActivity()
        {
            var repo = new MockActivityRepository();
            repo.SeedActivities(new Activity { Id = 1, Name = "A1" });
            var service = new ActivityService(repo);
            await service.UpdateAsync(new ActivityViewModel { Id = 1, Name = "A1_Updated" });
            Assert.Equal(1, repo.UpdateCallCount);
            var updated = await repo.GetByIdAsync(1);
            Assert.Equal("A1_Updated", updated?.Name);
        }

        [Fact]
        public async Task UpdateAsync_NotExisting_ThrowsException()
        {
            var repo = new MockActivityRepository();
            var service = new ActivityService(repo);
            await Assert.ThrowsAsync<ActivityNotFoundException>(() => service.UpdateAsync(new ActivityViewModel { Id = 99, Name = "X" }));
        }

        [Fact]
        public async Task DeleteAsync_Existing_DeletesActivity()
        {
            var repo = new MockActivityRepository();
            repo.SeedActivities(new Activity { Id = 1, Name = "A1" });
            var service = new ActivityService(repo);
            await service.DeleteAsync(1);
            Assert.Equal(1, repo.DeleteCallCount);
            Assert.Null(await repo.GetByIdAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_NotExisting_ThrowsException()
        {
            var repo = new MockActivityRepository();
            var service = new ActivityService(repo);
            await Assert.ThrowsAsync<ActivityNotFoundException>(() => service.DeleteAsync(99));
        }

        [Fact]
        public async Task AddAsync_Multiple_IncrementsCount()
        {
            var repo = new MockActivityRepository();
            var service = new ActivityService(repo);
            await service.AddAsync(new ActivityViewModel { Name = "A1" });
            await service.AddAsync(new ActivityViewModel { Name = "A2" });
            Assert.Equal(2, repo.AddCallCount);
        }

        [Fact]
        public async Task GetAllAsync_AfterAdd_ReturnsAll()
        {
            var repo = new MockActivityRepository();
            var service = new ActivityService(repo);
            await service.AddAsync(new ActivityViewModel { Name = "A1" });
            var all = await service.GetAllAsync();
            Assert.Single(all);
        }

        [Fact]
        public async Task DeleteAsync_Multiple_CorrectlyDeletes()
        {
            var repo = new MockActivityRepository();
            repo.SeedActivities(new Activity { Id = 1 }, new Activity { Id = 2 });
            var service = new ActivityService(repo);
            await service.DeleteAsync(1);
            await service.DeleteAsync(2);
            var all = await service.GetAllAsync();
            Assert.Empty(all);
        }
        
        [Fact]
        public async Task GetByIdAsync_DifferentIds_ReturnsCorrect()
        {
            var repo = new MockActivityRepository();
            repo.SeedActivities(new Activity { Id = 1, Name = "A1" }, new Activity { Id = 2, Name = "A2" });
            var service = new ActivityService(repo);
            var r1 = await service.GetByIdAsync(1);
            var r2 = await service.GetByIdAsync(2);
            Assert.Equal("A1", r1.Name);
            Assert.Equal("A2", r2.Name);
        }

        [Fact]
        public async Task UpdateAsync_MaintainsId()
        {
            var repo = new MockActivityRepository();
            repo.SeedActivities(new Activity { Id = 5, Name = "A5" });
            var service = new ActivityService(repo);
            await service.UpdateAsync(new ActivityViewModel { Id = 5, Name = "NewName" });
            var item = await repo.GetByIdAsync(5);
            Assert.Equal(5, item?.Id);
            Assert.Equal("NewName", item?.Name);
        }
    }
}
