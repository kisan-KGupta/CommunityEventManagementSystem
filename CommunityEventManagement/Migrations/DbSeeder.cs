using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BCrypt.Net;
using CommunityEventManagement.Models.Domains;
using CommunityEventManagement.Models.Domains.Auth;

namespace CommunityEventManagement.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDatabaseAsync(IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<AppDbContext>();

                // Ensure the database is created and migrations are applied
                await context.Database.MigrateAsync();

                // 1. Seed Default Administrator Account
                var existingAdmin = await context.Users.FirstOrDefaultAsync(u => u.Role == "Admin");
                if (existingAdmin == null)
                {
                    var adminUser = new User
                    {
                        FullName = "System Administrator",
                        Email = "admin@event.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                        Role = "Admin"
                    };

                    await context.Users.AddAsync(adminUser);
                    await context.SaveChangesAsync();

                    var adminProfile = new AdminProfile
                    {
                        UserId = adminUser.Id,
                        Position = "Event Director"
                    };

                    await context.AdminProfiles.AddAsync(adminProfile);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Default admin seeded successfully.");
                }

                // 2. Seed Default Venues
                if (!await context.Venues.AnyAsync())
                {
                    var venues = new List<Venue>
                    {
                        new Venue { Name = "Grand City Hall", Address = "101 Civic Plaza", Capacity = 150 },
                        new Venue { Name = "Central Park Amphitheater", Address = "200 Greenery Way", Capacity = 500 },
                        new Venue { Name = "Tech Hub Conference Center", Address = "303 Innovation Boulevard", Capacity = 60 }
                    };

                    await context.Venues.AddRangeAsync(venues);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Default venues seeded.");
                }

                // 3. Seed Default Activities
                if (!await context.Activities.AnyAsync())
                {
                    var activities = new List<Activity>
                    {
                        new Activity { Name = "Advanced ASP.NET Core Workshop", Type = "Workshop", Description = "Interactive coding lab on Blazor Server." },
                        new Activity { Name = "Keynote Address: Future of AI", Type = "Talk", Description = "A fascinating talk on agentic AI capabilities." },
                        new Activity { Name = "Community Board Games", Type = "Game", Description = "Social games to network with community members." }
                    };

                    await context.Activities.AddRangeAsync(activities);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Default activities seeded.");
                }

                // 4. Seed Default Event
                if (!await context.Events.AnyAsync())
                {
                    var venue = await context.Venues.FirstAsync();
                    var activity = await context.Activities.FirstAsync();

                    var defaultEvent = new Event
                    {
                        Name = "Summer Tech Festival 2026",
                        Date = DateTime.Now.AddDays(14),
                        Description = "Join us for our annual technology and networking festival featuring guest speakers and interactive workshops."
                    };

                    await context.Events.AddAsync(defaultEvent);
                    await context.SaveChangesAsync();

                    // Connect junctions
                    await context.EventVenues.AddAsync(new EventVenue { EventId = defaultEvent.Id, VenueId = venue.Id });
                    await context.EventActivities.AddAsync(new EventActivity { EventId = defaultEvent.Id, ActivityId = activity.Id });

                    await context.SaveChangesAsync();
                    Console.WriteLine("Default event seeded.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred during database seeding:");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
