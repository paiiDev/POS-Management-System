using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Domain.Interfaces;
using POS.Shared.DTOs.Auth;

namespace POS.App.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if(User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Home");
                } else
                {
                    return RedirectToAction("CreateSale", "Sale");

                }
            }
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill in all required fields.";
                return View("Login", request);
            }

            var result = await _authService.LoginAsync(request);
            if (!result.IsSuccess || result.Value == null)
            {
                ViewBag.Error = result.Error ?? "Invalid username or password";
                return View("Login", request);
            }

            var principal = result.Value;
            var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow
                };

             await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            var userRole = principal.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

            if (userRole == "Admin")
            {
                return RedirectToAction("Index", "Home");


            }
            return RedirectToAction("CreateSale", "Sale");

        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await PerformLogout();
            return RedirectToAction("Login", "Auth");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutConfirmed()
        {
            await PerformLogout();
            return RedirectToAction("Login", "Auth");
        }

        private async Task PerformLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            // Clear all cookies explicitly
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
