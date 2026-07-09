using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Models.Domains.Auth;

namespace CommunityEventManagement.Tests.MocksRepo
{
    public class MockUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();
        private readonly List<Participant> _participants = new();
        private readonly List<AdminProfile> _admins = new();

        public int AddUserCallCount { get; private set; }
        public int AddParticipantCallCount { get; private set; }
        public int AddAdminCallCount { get; private set; }

        public void SeedUsers(params User[] users)
        {
            _users.Clear();
            _users.AddRange(users);
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }

        public Task<User?> GetByIdAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task AddAsync(User user)
        {
            AddUserCallCount++;
            user.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task AddParticipantProfileAsync(Participant profile)
        {
            AddParticipantCallCount++;
            profile.UserId = _participants.Count > 0 ? _participants.Max(p => p.UserId) + 1 : 1;
            _participants.Add(profile);
            return Task.CompletedTask;
        }

        public Task AddAdminProfileAsync(AdminProfile profile)
        {
            AddAdminCallCount++;
            profile.UserId = _admins.Count > 0 ? _admins.Max(a => a.UserId) + 1 : 1;
            _admins.Add(profile);
            return Task.CompletedTask;
        }
    }
}
