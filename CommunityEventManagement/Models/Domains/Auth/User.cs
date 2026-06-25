using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommunityEventManagement.Models.Domains.Auth
{
    // OOP Concept: Abstraction - Abstract class defining common properties but cannot be instantiated directly.
    // OOP Concept: Encapsulation - Using public properties with auto-implemented private backing fields to protect data state.
    // OOP: Multi-level Inheritance (BaseUser -> User)
    public abstract class BaseUser
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }

    // OOP: Inheritance - 'User' class inherits from 'BaseUser'
    public class User : BaseUser
    {
        [Required]
        public string Role { get; set; } = string.Empty; // e.g., "Admin", "Participant"

        // OOP: Method Overloading (Signature 1)
        public void UpdateEmail(string newEmail)
        {
            Email = newEmail;
        }

        // OOP: Method Overloading (Signature 2 - different parameters)
        public void UpdateEmail(string newEmail, bool forceUpdate)
        {
            if (forceUpdate)
            {
                Email = newEmail;
            }
        }
    }

    public abstract class UserProfile
    {
        [Required]
        [Key]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        // OOP: Method Overriding (Base method for polymorphism)
        public virtual string GetProfileDetails()
        {
            return "Standard Profile";
        }
    }
}
