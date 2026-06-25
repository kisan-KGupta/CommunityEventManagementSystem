using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;
using CommunityEventManagement.Data.Repositories;
using CommunityEventManagement.Models.Domains.Auth;
using CommunityEventManagement.Models.ViewModels;

namespace CommunityEventManagement.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginViewModel model);
        Task<bool> RegisterAsync(RegisterViewModel model);
        Task LogoutAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            var user = await _userRepository.GetByEmailAsync(model.Email);
            if (user == null)
                return false;

            // Verify password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                return false;

            // Generate user claims for role-based authorization
            var principal = CreateClaimsPrincipal(user);

            // Sign the user in
            await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return true;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            var existing = await _userRepository.GetByEmailAsync(model.Email);
            if (existing != null)
                return false; // User already exists

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = "Participant" // Default role for self-registration
            };

            await _userRepository.AddAsync(user);

            var participantProfile = new Participant
            {
                UserId = user.Id,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };

            await _userRepository.AddParticipantProfileAsync(participantProfile);
            return true;
        }

        public async Task LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        private static ClaimsPrincipal CreateClaimsPrincipal(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }
    }
}
