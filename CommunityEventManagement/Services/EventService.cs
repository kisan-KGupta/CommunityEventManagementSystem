using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Exceptions;
using CommunityEventManagement.Helper.Mapper;
using CommunityEventManagement.Models.ViewModels;

namespace CommunityEventManagement.Services
{
    public interface IEventService
    {
        Task<List<EventViewModel>> GetAllAsync();
        // OOP Concept: Polymorphism (Method Overloading)
        Task<List<EventViewModel>> GetAllAsync(string sortBy);
        Task<EventViewModel> GetByIdAsync(int id);
        Task AddAsync(EventViewModel model);
        Task UpdateAsync(EventViewModel model);
        Task DeleteAsync(int id);
        Task<EventViewModel> FindEventByNameAsync(string name);
    }

    // Base class to demonstrate Inheritance and Method Hiding
    public abstract class BaseService
    {
        // A common method for logging or common service task
        public virtual void LogAction(string action)
        {
            System.Console.WriteLine($"[BaseService] Log: {action}");
        }
    }

    public class EventService : BaseService, IEventService
    {
        private readonly IEventRepository _repository;

        public EventService(IEventRepository repository)
        {
            _repository = repository;
        }

        // OOP Concept: Polymorphism (Method Hiding using 'new' keyword)
        public new void LogAction(string action)
        {
            System.Console.WriteLine($"[EventService] Custom Log: {action}");
        }

        public async Task<List<EventViewModel>> GetAllAsync()
        {
            var events = await _repository.GetAllAsync();
            return events.Select(e => EventMapper.ToViewModel(e)).ToList();
        }

        // OOP Concept: Polymorphism (Method Overloading)
        // ALGORITHM: Bubble Sort
        public async Task<List<EventViewModel>> GetAllAsync(string sortBy)
        {
            var events = await GetAllAsync();

            if (sortBy == "Date")
            {
                // Bubble Sort Algorithm
                int n = events.Count;
                for (int i = 0; i < n - 1; i++)
                {
                    for (int j = 0; j < n - i - 1; j++)
                    {
                        if (events[j].StartDate > events[j + 1].StartDate)
                        {
                            // Swap
                            var temp = events[j];
                            events[j] = events[j + 1];
                            events[j + 1] = temp;
                        }
                    }
                }
            }

            return events;
        }

        // ALGORITHM: Binary Search
        public async Task<EventViewModel> FindEventByNameAsync(string name)
        {
            // Binary search requires a sorted list
            var allEvents = await GetAllAsync();
            var sortedEvents = allEvents.OrderBy(e => e.Name).ToList();

            int left = 0;
            int right = sortedEvents.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                int comparison = string.Compare(sortedEvents[mid].Name, name, System.StringComparison.OrdinalIgnoreCase);

                if (comparison == 0)
                {
                    return sortedEvents[mid]; // Found
                }
                if (comparison < 0)
                {
                    left = mid + 1; // Search right half
                }
                else
                {
                    right = mid - 1; // Search left half
                }
            }

            return null; // Not found
        }

        public async Task<EventViewModel> GetByIdAsync(int id)
        {
            var domain = await _repository.GetByIdAsync(id);
            if (domain == null)
                throw new EventNotFoundException(id);

            return EventMapper.ToViewModel(domain);
        }

        public async Task AddAsync(EventViewModel model)
        {
            LogAction("Adding new event"); // Uses the new hiding method

            // Validation: Server-side Duplicate Record Validation
            var exists = await _repository.ExistsByNameAsync(model.Name);
            if (exists)
                throw new DuplicateEventException(model.Name);

            var domain = EventMapper.ToDomainModel(model);

            await _repository.AddAsync(domain, model.SelectedVenueIds, model.SelectedActivityIds);
            model.Id = domain.Id;
        }

        public async Task UpdateAsync(EventViewModel model)
        {
            var domain = await _repository.GetByIdAsync(model.Id);
            if (domain == null)
                throw new EventNotFoundException(model.Id);

            EventMapper.UpdateDomainModel(domain, model);
            await _repository.UpdateAsync(domain, model.SelectedVenueIds, model.SelectedActivityIds);
        }

        public async Task DeleteAsync(int id)
        {
            var domain = await _repository.GetByIdAsync(id);
            if (domain == null)
                throw new EventNotFoundException(id);

            await _repository.DeleteAsync(id);
        }
    }
}
