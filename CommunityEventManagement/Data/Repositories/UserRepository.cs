using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CommunityEventManagement.Models.Domains.Auth;

namespace CommunityEventManagement.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task AddParticipantProfileAsync(Participant profile);
        Task AddAdminProfileAsync(AdminProfile profile);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddParticipantProfileAsync(Participant profile)
        {
            await _context.Participants.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task AddAdminProfileAsync(AdminProfile profile)
        {
            await _context.AdminProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }
    }
}
