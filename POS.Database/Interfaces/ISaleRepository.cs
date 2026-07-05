using POS.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Database.Interfaces
{
    public interface ISaleRepository
    {
        Task CreateSaleAsync(Sale sale);

        Task<(IEnumerable<Sale> sales, int totalCount)> GetAllPagedPaidSalesAsync(string? searchTerm, int pageNumber, int pageSize);

        Task<(IEnumerable<Sale> sales, int totalCount)> GetAllPagedVoidedSalesAsync(string? searchTerm, int pageNumber, int pageSize);


        Task<Sale?> GetSaleByIdAsync(int id);

        Task<Sale?> GetSaleForUpdateAsync(int id);

        Task UpdateSaleAsync(Sale sale);
    }
}
