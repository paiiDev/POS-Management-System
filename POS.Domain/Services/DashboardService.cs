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
           
            var startDate = DateTime.Today.AddDays(-6);
            var dailyRevenues = await _dashboardRepo.GetLast7DaysRevenueAsync(startDate);

            for (int i = 0; i < 7; i++)
            {
                var targetDate = startDate.AddDays(i);

                
                dashboardDto.RevenueLabels.Add(targetDate.ToString("ddd"));

                
                if (dailyRevenues.ContainsKey(targetDate))
                {
                    dashboardDto.RevenueData.Add(dailyRevenues[targetDate]);
                }
                else
                {
                    dashboardDto.RevenueData.Add(0);
                }
            }

       

            return Result<DashboardDto>.Success(dashboardDto);
        }
    }
}
