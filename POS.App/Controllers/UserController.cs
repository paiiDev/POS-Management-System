using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Domain.Interfaces;
using POS.Shared.DTOs.User;

namespace POS.App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            if (!users.IsSuccess)
            {
                ViewBag.Error = users.Error;
                return View();
            }
            return View(users.Value);
        }

        [HttpGet]
        public IActionResult CreateUser() { return View(); }

        [HttpPost]
        [ActionName("CreateUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserPostAsync(CreateUserDto request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill in all required fields.";
                return View("CreateUser", request);
            }
            var result = await _userService.CreateUserAsync(request);
            if (!result.IsSuccess)
            {
                ViewBag.Error = result.Error;
                return View("CreateUser", request);
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                ViewBag.Error = "User ID is required.";
            }

            var result = await _userService.GetUserByIdAsync(id);
            if (!result.IsSuccess)
            {
                ViewBag.Error = result.Error;
                return View();
            }

            return View(result.Value);
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPostAsync(UpdateUserDto request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill in all required fields.";
                return View("Edit", request);
            }
            var result = await _userService.UpdateUserAsync(request);
            if (!result.IsSuccess)
            {
                ViewBag.Error = result.Error;
                return View("Edit", request);
            }
            return RedirectToAction("Index");
        }
    }
}
