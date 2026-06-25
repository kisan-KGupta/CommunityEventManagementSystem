using CommunityEventManagement.Components;
using CommunityEventManagement.Data;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace CommunityEventManagement
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add standard services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Support MVC Controllers (needed for Login/Logout POST requests)
            builder.Services.AddControllersWithViews();

            // 1. Database Configuration (MySQL Connection via Pomelo EF Core provider)
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, serverVersion));

            // 2. HTTP Context Accessor for fetching current User identity in services
            builder.Services.AddHttpContextAccessor();

            // 3. Register Data Repositories (DAL)
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IVenueRepository, VenueRepository>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();

            // 4. Register Business Services (BLL)
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IVenueService, VenueService>();
            builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddScoped<RegistrationFacade>();

            // 5. Register Cookie-based Authentication & Authorization
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/auth/logout";
                    options.AccessDeniedPath = "/access-denied";
                });
            builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();

            var app = builder.Build();

            // 6. Database Seeding (Runs migrations and injects sample data)
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await DbSeeder.SeedDatabaseAsync(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAntiforgery();

            app.MapControllers();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            await app.RunAsync();
        }
    }
}
