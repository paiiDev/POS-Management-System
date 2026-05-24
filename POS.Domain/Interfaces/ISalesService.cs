using POS.Shared.Common;
using POS.Shared.DTOs.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Interfaces
{
    public interface ISalesService
    {
        Task<Result<SaleResponseDto>> CreateSaleAsync(CreateSaleDto dto);

        Task<Result<List<SaleDto>>> GetAllSalesAsync();

        Task<Result<SaleDto>> GetSaleByIdAsync(int id);

    }
}
