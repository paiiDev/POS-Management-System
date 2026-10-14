using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.Dashboard
{
    public class DashboardDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalInventory { get; set; }
        public int TotalCategories { get; set; }
        public int TotalTransactions { get; set; }

        public List<TopSellerDto> TopSellers { get; set; } = new List<TopSellerDto>();

        public List<string> RevenueLabels { get; set; } = new List<string>();
        public List<decimal> RevenueData { get; set; } = new List<decimal>();
        public List<decimal> CostData { get; set; } = new List<decimal>();
    }
}
