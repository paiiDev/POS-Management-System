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

        public async Task<Dictionary<DateTime, decimal>> GetLast7DaysRevenueAsync(DateTime startDate)
        {
            
            var sales = await _context.Sales
                .Where(s => s.Status == "Paid" && s.SaleDate >= startDate)
                .Select(s => new { s.SaleDate, s.TotalAmount })
                .ToListAsync();

            return sales
                .GroupBy(s => s.SaleDate.Date)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.TotalAmount));
        }

        public async Task<List<DailyFinancialDto>> GetDailyFinancialsAsync(DateTime startDate)
        {
            var dailyFinancials = await _context.SaleItems
                                    .Include(s => s.Sale)
                                    .Include(s => s.Product)
                                    .Where(s => s.Sale.Status == "Paid" && s.Sale.SaleDate >= startDate)
                                    .Select(s => new DailyFinancialDto
                                    {
                                        Date = s.Sale.SaleDate.Date,
                                        TotalRevenue = s.Quantity * s.Product.SellingPrice,
                                        TotalCost = s.Quantity * s.Product.CostPrice
                                    }) .ToListAsync();

            return dailyFinancials.GroupBy(x => x.Date)
                                  .Select(g => new DailyFinancialDto
                                  {
                                      Date = g.Key,
                                      TotalRevenue = g.Sum(x => x.TotalRevenue),
                                      TotalCost = g.Sum(x => x.TotalCost)
                                  })
                                  .OrderBy(x => x.Date)
                                  .ToList();
        }
    }
}
