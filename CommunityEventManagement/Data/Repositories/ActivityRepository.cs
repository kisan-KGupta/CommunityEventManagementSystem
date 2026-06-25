using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CommunityEventManagement.Models.Domains;

namespace CommunityEventManagement.Data.Repositories
{
    public interface IActivityRepository
    {
        Task<List<Activity>> GetAllAsync();
        Task<Activity?> GetByIdAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
        Task AddAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(int id);
    }

    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _context;

        public ActivityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Activity>> GetAllAsync()
        {
            return await _context.Activities.ToListAsync();
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _context.Activities.FindAsync(id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Activities.AnyAsync(a => a.Name.ToLower() == name.ToLower());
        }

        public async Task AddAsync(Activity activity)
        {
            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
