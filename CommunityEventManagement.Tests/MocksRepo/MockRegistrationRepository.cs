using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Models.Domains;

namespace CommunityEventManagement.Tests.MocksRepo
{
    public class MockRegistrationRepository : IRegistrationRepository
    {
        private readonly List<Registration> _registrations = new();

        public int AddCallCount { get; private set; }
        public int UpdateStatusCallCount { get; private set; }
        public int DeleteCallCount { get; private set; }
        public Registration? LastAddedRegistration { get; private set; }

        public void SeedRegistrations(params Registration[] registrations)
        {
            _registrations.Clear();
            _registrations.AddRange(registrations);
        }

        public Task<List<Registration>> GetAllAsync()
        {
            return Task.FromResult(_registrations.ToList());
        }

        public Task<Registration?> GetByIdAsync(int id)
        {
            var target = _registrations.FirstOrDefault(r => r.Id == id);
            return Task.FromResult(target);
        }

        public Task<List<Registration>> GetByParticipantIdAsync(int participantId)
        {
            var targets = _registrations.Where(r => r.ParticipantId == participantId).ToList();
            return Task.FromResult(targets);
        }

        public Task<bool> ExistsAsync(int participantId, int eventId)
        {
            var exists = _registrations.Any(r => r.ParticipantId == participantId && r.EventId == eventId);
            return Task.FromResult(exists);
        }

        public Task AddAsync(Registration registration)
        {
            AddCallCount++;
            registration.Id = _registrations.Count > 0 ? _registrations.Max(r => r.Id) + 1 : 1;
            LastAddedRegistration = registration;
            _registrations.Add(registration);
            return Task.CompletedTask;
        }

        public Task UpdateStatusAsync(int id, string status)
        {
            UpdateStatusCallCount++;
            var reg = _registrations.FirstOrDefault(r => r.Id == id);
            if (reg != null)
            {
                reg.Status = status;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            DeleteCallCount++;
            _registrations.RemoveAll(r => r.Id == id);
            return Task.CompletedTask;
        }
    }
}
