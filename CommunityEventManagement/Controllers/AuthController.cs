using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommunityEventManagement.Models.ViewModels;
using CommunityEventManagement.Services;

namespace CommunityEventManagement.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Redirect("/login?error=validation");
                }

                var success = await _authService.LoginAsync(model);
                if (!success)
                {
                    return Redirect("/login?error=invalid_credentials");
                }

                return Redirect("/");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login Exception: {ex.Message}");
                return Redirect("/login?error=server_error");
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Redirect("/register?error=validation");
                }

                var success = await _authService.RegisterAsync(model);
                if (!success)
                {
                    return Redirect("/register?error=email_exists");
                }

                // Auto login after registration
                var loginModel = new LoginViewModel { Email = model.Email, Password = model.Password };
                await _authService.LoginAsync(loginModel);

                return Redirect("/");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration Exception: {ex.Message}");
                return Redirect("/register?error=server_error");
            }
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _authService.LogoutAsync();
                return Redirect("/login");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout Exception: {ex.Message}");
                return Redirect("/");
            }
        }
    }
}
