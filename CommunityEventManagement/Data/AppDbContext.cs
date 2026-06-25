using Microsoft.EntityFrameworkCore;
using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Models.Domains.Auth;

namespace CommunityEventManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Authentication & Profile DbSets
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<AdminProfile> AdminProfiles { get; set; } = null!;
        public DbSet<Participant> Participants { get; set; } = null!;

        // Domain DbSets
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Venue> Venues { get; set; } = null!;
        public DbSet<Activity> Activities { get; set; } = null!;
        public DbSet<Registration> Registrations { get; set; } = null!;
        public DbSet<EventVenue> EventVenues { get; set; } = null!;
        public DbSet<EventActivity> EventActivities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure 1-to-1 relationship between User and UserProfile tables
            modelBuilder.Entity<AdminProfile>()
                .HasKey(a => a.UserId);

            modelBuilder.Entity<Participant>()
                .HasKey(p => p.UserId);

            // Configure composite key for EventVenue many-to-many relationship
            modelBuilder.Entity<EventVenue>()
                .HasKey(ev => new { ev.EventId, ev.VenueId });

            modelBuilder.Entity<EventVenue>()
                .HasOne(ev => ev.Event)
                .WithMany(e => e.EventVenues)
                .HasForeignKey(ev => ev.EventId);

            modelBuilder.Entity<EventVenue>()
                .HasOne(ev => ev.Venue)
                .WithMany(v => v.EventVenues)
                .HasForeignKey(ev => ev.VenueId);

            // Configure composite key for EventActivity many-to-many relationship
            modelBuilder.Entity<EventActivity>()
                .HasKey(ea => new { ea.EventId, ea.ActivityId });

            modelBuilder.Entity<EventActivity>()
                .HasOne(ea => ea.Event)
                .WithMany(e => e.EventActivities)
                .HasForeignKey(ea => ea.EventId);

            modelBuilder.Entity<EventActivity>()
                .HasOne(ea => ea.Activity)
                .WithMany(a => a.EventActivities)
                .HasForeignKey(ea => ea.ActivityId);

            // Seed Roles or set indexing if needed
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
