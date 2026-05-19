using Microsoft.AspNetCore.Mvc;
using POS.Domain.Interfaces;
using System.Threading.Tasks;

namespace POS.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }



        public async Task<IActionResult> Index()
        {
            var result = await _productService.GetAllProductsAsync();
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Error;
                return View();
            }
            return View(result.Value);
        }
    }
}
