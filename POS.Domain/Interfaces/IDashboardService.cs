using POS.Shared.Common;
using POS.Shared.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Interfaces
{
    public interface IDashboardService
    {
        Task<Result<DashboardDto>> GetDashboardDataAsync();

    }
}
