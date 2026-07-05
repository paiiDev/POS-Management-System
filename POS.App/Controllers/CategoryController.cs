using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Database.Entities;
using POS.Domain.Interfaces;
using POS.Shared.DTOs.Category;
using POS.Shared.DTOs.Product;
using System.Threading.Tasks;

namespace POS.App.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            int pageSize = 10;
            var result = await _categoryService.GetAllPagedCategoriesAsync(searchString, page, pageSize);
            if (!result.IsSuccess) 
            {
                TempData["ErrorMessage"] = result.Error;
                return View();
            }
            ViewBag.CurrentSearch = searchString;
            return View(result.Value);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
           return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDto request)
        {
            if(!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _categoryService.AddCategoryAsync(request);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return View(request);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async  Task<IActionResult> Edit(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return View(result.Error);
            }
            var dto = new UpdateCategoryDto { Id = result!.Value!.Id, Name = result.Value.Name };
            return View(dto);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryDto request)
        {
            var result = await _categoryService.UpdateCategoryAsync(request);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return View(request);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public  async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return View();
            }

            var dto = new CategoryDto
            {
                Id = result!.Value!.Id,
                Name = result!.Value!.Name,
            };
            return View(dto);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction("index");
            }
            return RedirectToAction("index");
        }
    }
}
