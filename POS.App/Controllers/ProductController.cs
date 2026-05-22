using Microsoft.AspNetCore.Mvc;
using POS.Database.Entities;
using POS.Domain.Interfaces;
using POS.Shared.DTOs.Category;
using POS.Shared.DTOs.Product;
using System.Runtime.InteropServices;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(productDto);
            }
            var result = await _productService.CreateProductAsync(productDto);
            if (!result.IsSuccess)
            {
                await LoadCategoriesAsync();
                ModelState.AddModelError(string.Empty, "Failed to create product: " + result.Error);
                return View(productDto);
            }
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Edit(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }
            
            var dto = new UpdateProductDto { 
                        Id = result.Value.Id, 
                        Name=result.Value.Name, 
                        Barcode=result.Value.Barcode, 
                        CostPrice=result.Value.CostPrice, 
                        SellingPrice=result.Value.SellingPrice,
                        StockQuantity =result.Value.StockQuantity, 
                        CategoryId=result.Value.CategoryId 
                        };

            await LoadCategoriesAsync();
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("SubmitEdit")]
        public async Task<IActionResult> Edit(UpdateProductDto request)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(request);
            }
            var result = await _productService.UpdateProductAsync(request);
            if (!result.IsSuccess)
            {
                await LoadCategoriesAsync();
                ModelState.AddModelError(string.Empty, "Failed to upate product: " + result.Error);
                return View(request);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }

            return View(result.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmedDelete(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _productService.DeleteProductAsync(id);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
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
                return;
            }

            ViewBag.Categories = result.Value;
        }
    }
}

