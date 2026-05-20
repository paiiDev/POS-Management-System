using Microsoft.AspNetCore.Mvc;
using POS.Domain.Interfaces;
using POS.Shared.DTOs.Category;
using POS.Shared.DTOs.Product;
using System.Threading.Tasks;

namespace POS.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }



        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAllProductsAsync();
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction("Error", "Home");
            }
            return View(result.Value);
        }


        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto productDto)
        {
            var result = await _productService.CreateProductAsync(productDto);
            if (!result.IsSuccess)
            {
                await LoadCategoriesAsync();
                ModelState.AddModelError(string.Empty, "Failed to create product: " + result.Error);
                return View(productDto);
            }
            return RedirectToAction(nameof(Index));

        }


    private async Task LoadCategoriesAsync()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            if (!result.IsSuccess)
            {
                ViewBag.Categories = new List<CategoryDto>();
                TempData["ErrorMessage"] = result.Error;
            }
                ViewBag.Categories = result.Value;



        }
    }
}

