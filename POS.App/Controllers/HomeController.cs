using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.App.Models;
using POS.Shared.DTOs.Dashboard;
using System.Diagnostics;

namespace POS.App.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var dashboardData = new DashboardDto
            {
                TotalRevenue = 1500000,
                TotalInventory = 91,
                TotalCategories = 135,
                TotalTransactions = 741,
                TopSellers = new List<TopSellerDto>
            {
                new TopSellerDto { ProductName = "Elderberry", UnitsSold = 121 },
                new TopSellerDto { ProductName = "Longan", UnitsSold = 111 },
                new TopSellerDto { ProductName = "Ackee", UnitsSold = 110 },
                new TopSellerDto { ProductName = "Miracle fruit", UnitsSold = 109 },
                new TopSellerDto { ProductName = "Mulberry", UnitsSold = 109 }
            }
            };

            return View(dashboardData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
