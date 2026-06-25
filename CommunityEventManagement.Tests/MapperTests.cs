using Xunit;
using CommunityEventManagement.Helper.Mapper;
using CommunityEventManagement.Models.Domains;
using System.Collections.Generic;
using System;

namespace CommunityEventManagement.Tests
{
    public class MapperTests
    {
        [Fact]
        public void EventMapper_ToViewModel_MapsAllFieldsCorrectly()
        {
            var eventDomain = new Event
            {
                Id = 1,
                Name = "Test Event",
                Date = new DateTime(2025, 1, 1),
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 1, 1).AddHours(2),
                Description = "A description",
                EventVenues = new List<EventVenue>(),
                EventActivities = new List<EventActivity>(),
                Registrations = new List<Registration>()
            };

            var viewModel = EventMapper.ToViewModel(eventDomain);

            Assert.Equal(1, viewModel.Id);
            Assert.Equal("Test Event", viewModel.Name);
            Assert.Equal("A description", viewModel.Description);
        }
    }
}
