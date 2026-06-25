using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Exceptions;
using CommunityEventManagement.Helper.Mapper;
using CommunityEventManagement.Models.ViewModels;

namespace CommunityEventManagement.Services
{
    public interface IVenueService
    {
        Task<List<VenueViewModel>> GetAllAsync();
        Task<VenueViewModel> GetByIdAsync(int id);
        Task AddAsync(VenueViewModel model);
        Task UpdateAsync(VenueViewModel model);
        Task DeleteAsync(int id);
    }

    public class VenueService : IVenueService
    {
        private readonly IVenueRepository _repository;

        public VenueService(IVenueRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<VenueViewModel>> GetAllAsync()
        {
            var venues = await _repository.GetAllAsync();
            return venues.Select(v => VenueMapper.ToViewModel(v)).ToList();
        }

        public async Task<VenueViewModel> GetByIdAsync(int id)
        {
            var domain = await _repository.GetByIdAsync(id);
            if (domain == null)
                throw new VenueNotFoundException(id);

            return VenueMapper.ToViewModel(domain);
        }

        public async Task AddAsync(VenueViewModel model)
        {
            var exists = await _repository.ExistsByNameAsync(model.Name);
            if (exists)
                throw new System.Exception($"A venue with name '{model.Name}' already exists.");

            var domain = VenueMapper.ToDomainModel(model);
            await _repository.AddAsync(domain);
            model.Id = domain.Id;
        }

        public async Task UpdateAsync(VenueViewModel model)
        {
            var domain = await _repository.GetByIdAsync(model.Id);
            if (domain == null)
                throw new VenueNotFoundException(model.Id);

            VenueMapper.UpdateDomainModel(domain, model);
            await _repository.UpdateAsync(domain);
        }

        public async Task DeleteAsync(int id)
        {
            var domain = await _repository.GetByIdAsync(id);
            if (domain == null)
                throw new VenueNotFoundException(id);

            await _repository.DeleteAsync(id);
        }
    }
}
