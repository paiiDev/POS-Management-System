using Microsoft.EntityFrameworkCore;
using POS.Database.Context;
using POS.Database.Interfaces;
using POS.Shared.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Database.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;
        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Sales.Where(s => s.Status == "Paid").SumAsync(s => s.TotalAmount);
        }

        public async Task<int> GetTotalInventoryAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<int> GetTotalCategoriesAsync()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task<int> GetTotalTransactionsAsync()
        {
            return await _context.Sales.Where(s => s.Status == "Paid").CountAsync();
        }

        public async Task<List<TopSellerDto>> GetTopSellersAsync(int count)
        {
            var topSellers = await _context.SaleItems
                .Where(s => s.Sale.Status == "Paid")
                .GroupBy(s => new { s.ProductId, s.Product.Name })
                .Select(g => new TopSellerDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    UnitsSold = g.Sum(s => s.Quantity)
                })
                .OrderByDescending(ts => ts.UnitsSold)
                .Take(count)
                .ToListAsync();
            return topSellers;
        }
    }
}
