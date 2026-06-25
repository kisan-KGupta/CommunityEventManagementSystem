using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Models.ViewModels;

namespace CommunityEventManagement.Helper.Mapper
{
    public static class VenueMapper
    {
        public static VenueViewModel ToViewModel(Venue domain)
        {
            return new VenueViewModel
            {
                Id = domain.Id,
                Name = domain.Name,
                Address = domain.Address,
                Capacity = domain.Capacity
            };
        }

        public static Venue ToDomainModel(VenueViewModel model)
        {
            return new Venue
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                Capacity = model.Capacity
            };
        }

        public static void UpdateDomainModel(Venue domain, VenueViewModel model)
        {
            domain.Name = model.Name;
            domain.Address = model.Address;
            domain.Capacity = model.Capacity;
        }
    }
}
