using Microsoft.AspNetCore.Mvc;
using POS.Database.Entities;
using POS.Domain.Interfaces;
using POS.Shared.DTOs.Category;
using System.Threading.Tasks;

namespace POS.App.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            if (!result.IsSuccess) 
            { 
                return View("Error", result.Error);
            }
            return View(result.Value);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto request)
        {
            var result = await _categoryService.AddCategoryAsync(request);
            if (!result.IsSuccess)
            {
                return View("Error", result.Error);
            }
            return RedirectToAction("Index");
        }   
        

        public async  Task<IActionResult> Edit(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (!result.IsSuccess)
            {
                return View("Error", result.Error);
            }
            var dto = new UpdateCategoryDto { Id = result!.Value!.Id, Name = result.Value.Name };
            return View(dto);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryDto request)
        {
            var result = await _categoryService.UpdateCategoryAsync(request);
            if (!result.IsSuccess)
            {
                return View("Error", result.Error);
            }
            return RedirectToAction("Index");
        }


        public  async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (!result.IsSuccess)
            {
                return View("Error", result.Error);
            }

            var dto = new CategoryDto
            {
                Id = result!.Value!.Id,
                Name = result!.Value!.Name,
            };
            return View(dto);
        }


        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result.IsSuccess)
            {
                return View("Error", result.Error);
            }
            return RedirectToAction("index");
        }
    }
}
