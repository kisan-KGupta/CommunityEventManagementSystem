using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Models.ViewModels;

namespace CommunityEventManagement.Helper.Mapper
{
    public static class ActivityMapper
    {
        public static ActivityViewModel ToViewModel(Activity domain)
        {
            return new ActivityViewModel
            {
                Id = domain.Id,
                Name = domain.Name,
                Type = domain.Type,
                Description = domain.Description
            };
        }

        public static Activity ToDomainModel(ActivityViewModel model)
        {
            return new Activity
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                Description = model.Description
            };
        }

        public static void UpdateDomainModel(Activity domain, ActivityViewModel model)
        {
            domain.Name = model.Name;
            domain.Type = model.Type;
            domain.Description = model.Description;
        }
    }
}
