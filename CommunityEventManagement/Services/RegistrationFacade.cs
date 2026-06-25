using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommunityEventManagement.Services
{
    // Design Pattern: Structural - Facade Pattern
    // Provides a simplified, higher-level interface to the complex subsystems (Registration and Event services).
    // Also implements DSA Concepts: Queue<T> for Waitlists, Stack<T> for Undo Operations, LinkedList<T> for Audit Logs.
    public class RegistrationFacade
    {
        private readonly IRegistrationService _registrationService;
        private readonly IEventService _eventService;

        // DSA Concept: Queue<T> used for real business logic (Waitlist processing)
        private readonly Queue<int> _waitlistQueue = new Queue<int>();

        // DSA Concept: Stack<T> used for real business logic (Undo last registration)
        private readonly Stack<int> _registrationHistoryStack = new Stack<int>();

        // DSA Concept: LinkedList<T> used for real business logic (Fast insertion of audit trail)
        private readonly LinkedList<string> _auditLogList = new LinkedList<string>();

        public RegistrationFacade(IRegistrationService registrationService, IEventService eventService)
        {
            _registrationService = registrationService;
            _eventService = eventService;
        }

        public async Task<string> RegisterOrWaitlistParticipantAsync(int participantId, int eventId)
        {
            try
            {
                // Attempt standard registration via subsystem
                await _registrationService.RegisterParticipantAsync(participantId, eventId);
                
                // Track operation in Stack for potential undo
                _registrationHistoryStack.Push(participantId);
                
                // Track operation in LinkedList for fast audit
                _auditLogList.AddLast($"[Success] Participant {participantId} registered for Event {eventId} at {DateTime.UtcNow}");
                return "Successfully registered for this event!";
            }
            catch (Exceptions.EventFullException)
            {
                // If full, add to Queue
                _waitlistQueue.Enqueue(participantId);
                _auditLogList.AddLast($"[Waitlist] Participant {participantId} added to waitlist for Event {eventId} at {DateTime.UtcNow}");
                return "Event is full. You have been added to the waitlist.";
            }
        }

        public async Task CancelRegistrationAsync(int registrationId)
        {
            await _registrationService.UpdateStatusAsync(registrationId, "Cancelled");
            _registrationHistoryStack.Push(registrationId);
            _auditLogList.AddLast($"[Cancel] Registration {registrationId} was cancelled at {DateTime.UtcNow}");
        }

        public async Task<string> UndoLastRegistrationAsync()
        {
            if (_registrationHistoryStack.Count > 0)
            {
                int lastRegistrationId = _registrationHistoryStack.Pop();
                await _registrationService.UpdateStatusAsync(lastRegistrationId, "Confirmed");
                _auditLogList.AddLast($"[Undo] Reverted cancellation for Registration {lastRegistrationId}");
                return $"Successfully undone cancellation for registration {lastRegistrationId}.";
            }
            return "No recent cancellations to undo.";
        }

        public IEnumerable<string> GetAuditLogs()
        {
            return _auditLogList;
        }
    }
}
