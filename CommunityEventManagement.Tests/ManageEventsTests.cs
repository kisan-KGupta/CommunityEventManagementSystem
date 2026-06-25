using System.Threading.Tasks;
using Xunit;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using CommunityEventManagement.Components.Pages.Admin;
using CommunityEventManagement.Services;
using CommunityEventManagement.Models.ViewModels;
using System.Collections.Generic;

namespace CommunityEventManagement.Tests
{
    public class ManageEventsTests : BunitContext
    {
        private class DummyEventService : IEventService
        {
            public Task AddAsync(EventViewModel model) => Task.CompletedTask;
            public Task DeleteAsync(int id) => Task.CompletedTask;
            public Task<EventViewModel> FindEventByNameAsync(string name) => Task.FromResult<EventViewModel>(null);
            public Task<List<EventViewModel>> GetAllAsync() => Task.FromResult(new List<EventViewModel>());
            public Task<List<EventViewModel>> GetAllAsync(string sortBy) => Task.FromResult(new List<EventViewModel>());
            public Task<EventViewModel> GetByIdAsync(int id) => Task.FromResult(new EventViewModel());
            public Task UpdateAsync(EventViewModel model) => Task.CompletedTask;
        }

        private class DummyVenueService : IVenueService
        {
            public Task<List<VenueViewModel>> GetAllAsync() => Task.FromResult(new List<VenueViewModel>());
            // Add stubs for other methods if required
            public Task AddAsync(VenueViewModel model) => Task.CompletedTask;
            public Task DeleteAsync(int id) => Task.CompletedTask;
            public Task<VenueViewModel> GetByIdAsync(int id) => Task.FromResult(new VenueViewModel());
            public Task UpdateAsync(VenueViewModel model) => Task.CompletedTask;
        }

        private class DummyActivityService : IActivityService
        {
            public Task<List<ActivityViewModel>> GetAllAsync() => Task.FromResult(new List<ActivityViewModel>());
            public Task AddAsync(ActivityViewModel model) => Task.CompletedTask;
            public Task DeleteAsync(int id) => Task.CompletedTask;
            public Task<ActivityViewModel> GetByIdAsync(int id) => Task.FromResult(new ActivityViewModel());
            public Task UpdateAsync(ActivityViewModel model) => Task.CompletedTask;
        }

        [Fact]
        public void ManageEvents_RendersCorrectly()
        {
            Services.AddSingleton<IEventService>(new DummyEventService());
            Services.AddSingleton<IVenueService>(new DummyVenueService());
            Services.AddSingleton<IActivityService>(new DummyActivityService());

            var cut = Render<ManageEvents>();

            Assert.Contains("Events List", cut.Markup);
        }
    }
}
