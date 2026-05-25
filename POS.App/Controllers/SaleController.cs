using Microsoft.AspNetCore.Mvc;
using POS.Domain.Interfaces;
using POS.Shared.DTOs.Sales;
using System.Threading.Tasks;

namespace POS.App.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISalesService _salesService;
        private readonly IProductService _productService;
        public SaleController(ISalesService salesService, IProductService productService)
        {
            _salesService = salesService;
            _productService = productService;
        }
        public async Task<IActionResult> CreateSale()
        {
            await LoadProducts();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSaleDto request)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input data.";
                await LoadProducts();
                return View("CreateSale", request);
            }

            var result = await _salesService.CreateSaleAsync(request);
            if (!result.IsSuccess)
            {
                ViewBag.Error = result.Error;
                await LoadProducts();
                return View("CreateSale", request);
            }

            var saleId = result.Value.Id;

            return RedirectToAction("ConfirmSale", new { id = saleId });
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmSale(int id)
        {
            var result = await _salesService.GetSaleByIdAsync(id);
            if (!result.IsSuccess || result.Value is null)
            {
                return NotFound(result.Error);
            }
            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> ViewSaleTransactions()
        {
            var result = await _salesService.GetAllSalesAsync();
            if (!result.IsSuccess || result.Value is null)
            {
                ViewBag.Error = result.Error;
                return View(new List<SaleDto>());
            }
            return View(result.Value);
        }


        [HttpGet]
        public async Task<IActionResult> SaleDetails(int id)
        {
            var result = await _salesService.GetSaleByIdAsync(id);
            if (!result.IsSuccess || result.Value is null)
            {
                return NotFound(result.Error);
            }

            return View(result.Value);
        }

        private async Task LoadProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            if (!products.IsSuccess)
            {
                ViewBag.Error = "Failed to retrieve products data.";
                return;
            }

            ViewBag.Products = products.Value;
        }
    }
}
