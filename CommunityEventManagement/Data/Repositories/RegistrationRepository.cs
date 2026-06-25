using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CommunityEventManagement.Models.Domains;

namespace CommunityEventManagement.Data.Repositories
{
    public interface IRegistrationRepository
    {
        Task<List<Registration>> GetAllAsync();
        Task<Registration?> GetByIdAsync(int id);
        Task<List<Registration>> GetByParticipantIdAsync(int participantId);
        Task<bool> ExistsAsync(int participantId, int eventId);
        Task AddAsync(Registration registration);
        Task UpdateStatusAsync(int id, string status);
        Task DeleteAsync(int id);
    }

    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly AppDbContext _context;

        public RegistrationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Registration>> GetAllAsync()
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.Participant).ThenInclude(p => p.User)
                .ToListAsync();
        }

        public async Task<Registration?> GetByIdAsync(int id)
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .Include(r => r.Participant).ThenInclude(p => p.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Registration>> GetByParticipantIdAsync(int participantId)
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .Where(r => r.ParticipantId == participantId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int participantId, int eventId)
        {
            return await _context.Registrations
                .AnyAsync(r => r.ParticipantId == participantId && r.EventId == eventId);
        }

        public async Task AddAsync(Registration registration)
        {
            await _context.Registrations.AddAsync(registration);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var reg = await _context.Registrations.FindAsync(id);
            if (reg != null)
            {
                reg.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var reg = await _context.Registrations.FindAsync(id);
            if (reg != null)
            {
                _context.Registrations.Remove(reg);
                await _context.SaveChangesAsync();
            }
        }
    }
}
