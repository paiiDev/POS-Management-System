using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Domain.Interfaces;
using POS.Shared.DTOs.Pagination;
using POS.Shared.DTOs.Sales;
using POS.Shared.DTOs.VoidLog;
using POS.Shared.Extensions;
using System.Threading.Tasks;

namespace POS.App.Controllers
{
    [Authorize]
    public class SaleController : Controller
    {
        private readonly ISalesService _salesService;
        private readonly IProductService _productService;
        public SaleController(ISalesService salesService, IProductService productService)
        {
            _salesService = salesService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> CreateSale(string searchString, int page = 1)
        {
            await LoadProducts(page, searchString);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string searchString, CreateSaleDto request, int page = 1)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input data.";
                await LoadProducts(page, searchString);
                return View("CreateSale", request);
            }

            request.UserId = User.GetUserId();
            var result = await _salesService.CreateSaleAsync(request);
            if (!result.IsSuccess)
            {
                ViewBag.Error = result.Error;
                await LoadProducts(page, searchString);
                return View("CreateSale", request);
            }

            var saleId = result.Value.Id;

            return RedirectToAction("ConfirmSale", new { id = saleId });
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmSale(int id, string from = "Create")
        {
            var result = await _salesService.GetSaleByIdAsync(id);
            if (!result.IsSuccess || result.Value is null)
            {
                return NotFound(result.Error);
            }
            ViewBag.From = from;
            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> ViewSaleTransactions(int page = 1)
        {
            int pageSize = 10;
            var result = await _salesService.GetAllPagedPaidSalesAsync(page, pageSize);
            if (!result.IsSuccess || result.Value is null)
            {
                ViewBag.Error = result.Error;
                return View(new PagedResult<SaleDto>());
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VoidedSaleTransactions(int page = 1)
        {
            int pageSize = 10;
            var result = await _salesService.GetAllPagedVoidedSalesAsync(page, pageSize);
            if (!result.IsSuccess || result.Value is null)
            {
                ViewBag.Error = result.Error;
                return View(new PagedResult<SaleDto>());
            }
            return View(result.Value);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VoidedSaleDetails(int Id)
        {
            var result = await _salesService.GetVoidLogBySaleIdAsync(Id);
            if(!result.IsSuccess || result.Value is null) { return NotFound(result.Error); }
            return View(result.Value);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VoidSale(int id)
        {
           if(id <= 0)
            {
                return BadRequest("Sale ID is required.");
            }

           var sale = await _salesService.GetSaleByIdAsync(id);
            if (!sale.IsSuccess || sale.Value is null)
            {
                return NotFound(sale.Error);
            }


            return View("ConfirmSale", sale.Value);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmVoidSale(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Sale ID is required.");
            }

            var sale = await _salesService.GetSaleByIdAsync(id);
            if(!sale.IsSuccess || sale.Value is null)
            {
                return NotFound(sale.Error);
            }

            return View(sale.Value);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VoidSaleConfirmed(VoidLogDto request)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.Reason))
            {
                ViewBag.Error = "Please provide a valid reason.";
                var sale = await _salesService.GetSaleByIdAsync(request.SaleId);
                if (!sale.IsSuccess || sale.Value is null)
                {
                    return NotFound(sale.Error);
                }

                return View("ConfirmVoidSale", sale.Value);
            }

            if (request.SaleId <= 0)
            {
                return BadRequest("Sale ID is required.");
            }

            var result = await _salesService.CreateVoidLogAsync(request);

            if (!result.IsSuccess)
            {
                ViewBag.Error = result.Error;
                var sale = await _salesService.GetSaleByIdAsync(request.SaleId);
                if (!sale.IsSuccess || sale.Value is null)
                {
                    return NotFound(sale.Error);
                }
                return View("ConfirmVoidSale", sale.Value);
            }
            return RedirectToAction("VoidedSaleTransactions");
        }


        private async Task LoadProducts(int page, string? searchString)
        {
            int pageSize = 8;
            var result = await _productService.GetProductsPagedAsync(searchString, page, pageSize);
            if (!result.IsSuccess)
            {
                ViewBag.Error = "Failed to retrieve products data.";
                return;
            }

            ViewBag.Products = result.Value;
            ViewBag.CurrentSearch = searchString;
        }
    }
}
