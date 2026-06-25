using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Exceptions;
using CommunityEventManagement.Helper.Mapper;
using CommunityEventManagement.Models.ViewModels;

namespace CommunityEventManagement.Services
{
    public interface IActivityService
    {
        Task<List<ActivityViewModel>> GetAllAsync();
        Task<ActivityViewModel> GetByIdAsync(int id);
        Task AddAsync(ActivityViewModel model);
        Task UpdateAsync(ActivityViewModel model);
        Task DeleteAsync(int id);
    }

    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repository;

        public ActivityService(IActivityRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ActivityViewModel>> GetAllAsync()
        {
            var activities = await _repository.GetAllAsync();
            return activities.Select(a => ActivityMapper.ToViewModel(a)).ToList();
        }

        public async Task<ActivityViewModel> GetByIdAsync(int id)
        {
            var domain = await _repository.GetByIdAsync(id);
            if (domain == null)
                throw new ActivityNotFoundException(id);

            return ActivityMapper.ToViewModel(domain);
        }

        public async Task AddAsync(ActivityViewModel model)
        {
            var exists = await _repository.ExistsByNameAsync(model.Name);
            if (exists)
                throw new System.Exception($"An activity with name '{model.Name}' already exists.");

            var domain = ActivityMapper.ToDomainModel(model);
            await _repository.AddAsync(domain);
            model.Id = domain.Id;
        }

        public async Task UpdateAsync(ActivityViewModel model)
        {
            var domain = await _repository.GetByIdAsync(model.Id);
            if (domain == null)
                throw new ActivityNotFoundException(model.Id);

            ActivityMapper.UpdateDomainModel(domain, model);
            await _repository.UpdateAsync(domain);
        }

        public async Task DeleteAsync(int id)
        {
            var domain = await _repository.GetByIdAsync(id);
            if (domain == null)
                throw new ActivityNotFoundException(id);

            await _repository.DeleteAsync(id);
        }
    }
}
