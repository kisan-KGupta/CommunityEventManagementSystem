using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Exceptions;
using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Models.ViewModels;

namespace CommunityEventManagement.Services
{
    public interface IRegistrationService
    {
        Task<List<RegistrationViewModel>> GetAllAsync();
        Task<List<RegistrationViewModel>> GetByParticipantIdAsync(int participantId);
        Task RegisterParticipantAsync(int participantId, int eventId);
        Task UpdateStatusAsync(int id, string status);
        Task CancelRegistrationAsync(int id);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IEventRepository _eventRepository;

        public RegistrationService(
            IRegistrationRepository registrationRepository,
            IEventRepository eventRepository)
        {
            _registrationRepository = registrationRepository;
            _eventRepository = eventRepository;
        }

        public async Task<List<RegistrationViewModel>> GetAllAsync()
        {
            var registrations = await _registrationRepository.GetAllAsync();
            return registrations.Select(r => MapToViewModel(r)).ToList();
        }

        public async Task<List<RegistrationViewModel>> GetByParticipantIdAsync(int participantId)
        {
            var registrations = await _registrationRepository.GetByParticipantIdAsync(participantId);
            return registrations.Select(r => MapToViewModel(r)).ToList();
        }

        public async Task RegisterParticipantAsync(int participantId, int eventId)
        {
            // 1. Check if registration already exists
            var exists = await _registrationRepository.ExistsAsync(participantId, eventId);
            if (exists)
                throw new DuplicateRegistrationException();

            // 2. Fetch event to inspect capacity limits
            var targetEvent = await _eventRepository.GetByIdAsync(eventId);
            if (targetEvent == null)
                throw new EventNotFoundException(eventId);

            // 3. Sum the capacities of all Venues associated with this Event
            int totalCapacity = targetEvent.EventVenues.Sum(ev => ev.Venue?.Capacity ?? 0);

            // If no capacity is set, or total capacity is zero, default to a sensible number or throw
            if (totalCapacity > 0)
            {
                int currentRegistrationsCount = targetEvent.Registrations.Count(r => r.Status == "Confirmed");
                if (currentRegistrationsCount >= totalCapacity)
                {
                    throw new EventFullException(targetEvent.Name);
                }
            }

            // 4. Create and save registration
            var registration = new Registration
            {
                EventId = eventId,
                ParticipantId = participantId,
                RegistrationDate = DateTime.UtcNow,
                Status = "Confirmed"
            };

            await _registrationRepository.AddAsync(registration);
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var reg = await _registrationRepository.GetByIdAsync(id);
            if (reg == null)
                throw new Exception("Registration record not found");

            await _registrationRepository.UpdateStatusAsync(id, status);
        }

        public async Task CancelRegistrationAsync(int id)
        {
            var reg = await _registrationRepository.GetByIdAsync(id);
            if (reg == null)
                throw new Exception("Registration record not found");

            await _registrationRepository.UpdateStatusAsync(id, "Cancelled");
        }

        private static RegistrationViewModel MapToViewModel(Registration r)
        {
            return new RegistrationViewModel
            {
                Id = r.Id,
                EventId = r.EventId,
                EventName = r.Event?.Name ?? string.Empty,
                EventDate = r.Event?.Date ?? DateTime.MinValue,
                ParticipantId = r.ParticipantId,
                ParticipantName = r.Participant?.User?.FullName ?? string.Empty,
                ParticipantEmail = r.Participant?.User?.Email ?? string.Empty,
                RegistrationDate = r.RegistrationDate,
                Status = r.Status
            };
        }
    }
}
