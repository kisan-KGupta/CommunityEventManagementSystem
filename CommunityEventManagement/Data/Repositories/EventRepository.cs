using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CommunityEventManagement.Models.Domains;

namespace CommunityEventManagement.Data.Repositories
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllAsync();
        Task<Event?> GetByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
        Task AddAsync(Event communityEvent, List<int> venueIds, List<int> activityIds);
        Task UpdateAsync(Event communityEvent, List<int> venueIds, List<int> activityIds);
        Task DeleteAsync(int id);
    }

    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetAllAsync()
        {
            return await _context.Events
                .Include(e => e.EventVenues).ThenInclude(ev => ev.Venue)
                .Include(e => e.EventActivities).ThenInclude(ea => ea.Activity)
                .Include(e => e.Registrations)
                .ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.EventVenues).ThenInclude(ev => ev.Venue)
                .Include(e => e.EventActivities).ThenInclude(ea => ea.Activity)
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Events.AnyAsync(e => e.Name.ToLower() == name.ToLower());
        }

        public async Task AddAsync(Event communityEvent, List<int> venueIds, List<int> activityIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Events.AddAsync(communityEvent);
                await _context.SaveChangesAsync(); // Generates the Event ID

                // Add many-to-many Venues
                foreach (var vId in venueIds)
                {
                    await _context.EventVenues.AddAsync(new EventVenue { EventId = communityEvent.Id, VenueId = vId });
                }

                // Add many-to-many Activities
                foreach (var aId in activityIds)
                {
                    await _context.EventActivities.AddAsync(new EventActivity { EventId = communityEvent.Id, ActivityId = aId });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(Event communityEvent, List<int> venueIds, List<int> activityIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Events.Update(communityEvent);

                // Remove existing relationships
                var existingVenues = _context.EventVenues.Where(ev => ev.EventId == communityEvent.Id);
                _context.EventVenues.RemoveRange(existingVenues);

                var existingActivities = _context.EventActivities.Where(ea => ea.EventId == communityEvent.Id);
                _context.EventActivities.RemoveRange(existingActivities);

                await _context.SaveChangesAsync();

                // Re-add relationships
                foreach (var vId in venueIds)
                {
                    await _context.EventVenues.AddAsync(new EventVenue { EventId = communityEvent.Id, VenueId = vId });
                }

                foreach (var aId in activityIds)
                {
                    await _context.EventActivities.AddAsync(new EventActivity { EventId = communityEvent.Id, ActivityId = aId });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var communityEvent = await _context.Events.FindAsync(id);
            if (communityEvent != null)
            {
                _context.Events.Remove(communityEvent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
