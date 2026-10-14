using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Core.Interfaces.IServices;
using POS.Shared.DTOs.Auth;
using System.Security.Claims;

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
            if (User.Identity != null && User.Identity.IsAuthenticated && !string.IsNullOrEmpty(User!.Identity!.Name))
            {
                if(User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Home");
                } else if(User.IsInRole("Manager") || User.IsInRole("Cashier"))
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

            var user = result.Value;

            var claims = new List<Claim>
           {
               new Claim(ClaimTypes.Name, user.UserName),
                new Claim("FullName", user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
           };

           var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
           var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme ,principal, authProperties);
            return RedirectToAction("Index", "Home");

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
