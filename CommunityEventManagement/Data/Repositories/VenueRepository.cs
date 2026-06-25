using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CommunityEventManagement.Models.Domains;

namespace CommunityEventManagement.Data.Repositories
{
    public interface IVenueRepository
    {
        Task<List<Venue>> GetAllAsync();
        Task<Venue?> GetByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
        Task AddAsync(Venue venue);
        Task UpdateAsync(Venue venue);
        Task DeleteAsync(int id);
    }

    public class VenueRepository : IVenueRepository
    {
        private readonly AppDbContext _context;

        public VenueRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Venue>> GetAllAsync()
        {
            return await _context.Venues.ToListAsync();
        }

        public async Task<Venue?> GetByIdAsync(int id)
        {
            return await _context.Venues.FindAsync(id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Venues.AnyAsync(v => v.Name.ToLower() == name.ToLower());
        }

        public async Task AddAsync(Venue venue)
        {
            await _context.Venues.AddAsync(venue);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Venue venue)
        {
            _context.Venues.Update(venue);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue != null)
            {
                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
            }
        }
    }
}
