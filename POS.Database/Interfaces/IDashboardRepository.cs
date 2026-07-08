using POS.Shared.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POS.Database.Interfaces
{
    public interface IDashboardRepository
    {
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetTotalInventoryAsync();
        Task<int> GetTotalCategoriesAsync();
        Task<int> GetTotalTransactionsAsync();
        Task<List<TopSellerDto>> GetTopSellersAsync(int count);
        Task<Dictionary<DateTime, decimal>> GetLast7DaysRevenueAsync(DateTime startDate);

    }
}
