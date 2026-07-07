using POS.Database.Interfaces;
using POS.Domain.Interfaces;
using POS.Shared.Common;
using POS.Shared.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepo;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepo = dashboardRepository;
        }

  
        public async Task<Result<DashboardDto>> GetDashboardDataAsync()
        {
            var dashboardDto = new DashboardDto
            {
                TotalRevenue = await _dashboardRepo.GetTotalRevenueAsync(),
                TotalInventory = await _dashboardRepo.GetTotalInventoryAsync(),
                TotalCategories = await _dashboardRepo.GetTotalCategoriesAsync(),
                TotalTransactions = await _dashboardRepo.GetTotalTransactionsAsync(),
                TopSellers = await _dashboardRepo.GetTopSellersAsync(5)
            };

            return Result<DashboardDto>.Success(dashboardDto);
        }
    }
}
