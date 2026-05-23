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

        Task<List<Sale>> GetAllSalesAsync();

        Task<Sale?> GetSaleByIdAsync(int id);
    }
}
