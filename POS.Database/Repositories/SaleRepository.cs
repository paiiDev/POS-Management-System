using Microsoft.EntityFrameworkCore;
using POS.Database.Context;
using POS.Database.Entities;
using POS.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Database.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _dbContext;
        public SaleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task CreateSaleAsync(Sale sale)
        {
             _dbContext.Sales.Add(sale);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<List<Sale>> GetAllSalesAsync()
        {
            return await _dbContext.Sales.Include(s => s.SaleItems).ThenInclude(si => si.Product).AsNoTracking().OrderByDescending(s => s.SaleDate).ToListAsync();
        }


        public async Task<Sale?> GetSaleByIdAsync(int id)
        {
            return await _dbContext.Sales.Include(s => s.SaleItems).ThenInclude(si => si.Product).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Sale?>  GetSaleForUpdateAsync(int id)
        {
            return await _dbContext.Sales.Include(s => s.SaleItems).ThenInclude(si => si.Product).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            var existingSale = await _dbContext.Sales.FirstOrDefaultAsync(x => x.Id == sale.Id);
            if (existingSale != null)
            {

                existingSale.Id = sale.Id;
                existingSale.InvoiceNo = sale.InvoiceNo;
                existingSale.TotalAmount = sale.TotalAmount;
                existingSale.SaleDate = sale.SaleDate;
                existingSale.UserId = sale.UserId;
                existingSale.Status = sale.Status;

                 _dbContext.Sales.Update(existingSale);
                await _dbContext.SaveChangesAsync();

            }
        }
        }
}
