using Microsoft.AspNetCore.Mvc;
using POS.Domain.Interfaces;
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
    }
}
