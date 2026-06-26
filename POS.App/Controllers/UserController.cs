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
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10; 
            var users = await _userService.GetPagedUsersAsync(page, pageSize);
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
            TempData["SuccessMessage"] = "User account created successfully";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                TempData["ErrorMessage"] = "User ID is required.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userService.GetUserByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }

            var user = result.Value!;
            return View(new UpdateUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Role = user.Role
            });
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
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

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "User deleted successfully";
            return RedirectToAction("index");
        }
    }
}
