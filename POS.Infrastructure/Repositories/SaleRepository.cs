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


        public async Task<(IEnumerable<Sale> sales, int totalCount)> GetAllPagedPaidSalesAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Max(pageSize, 1);

            var query = _dbContext.Sales.Where(s => s.Status == "Paid");

            if(!string.IsNullOrEmpty(searchTerm))
            {
                var keywords = searchTerm.Trim();
                query = query.Where(s => s.InvoiceNo.Contains(keywords));
            }

            var totalCount = await query.CountAsync();

             var sales = await query
                .IgnoreQueryFilters()
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
                .Include(u => u.User)
                .AsNoTracking()
                .OrderByDescending(s => s.SaleDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (sales, totalCount);
        }

        public async Task<(IEnumerable<Sale> sales, int totalCount)> GetAllPagedVoidedSalesAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Max(pageSize, 1);

            var query = _dbContext.Sales.Where(s => s.Status == "Voided");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var keywords = searchTerm.Trim();
                query = query.Where(s => s.InvoiceNo.Contains(keywords));
            }

            var totalCount = await query.CountAsync();

            var sales = await query
                .IgnoreQueryFilters()
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
                .Include(u => u.User)
                .Where(s => s.Status == "Voided")
                .AsNoTracking()
                .OrderByDescending(s => s.SaleDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (sales, totalCount);
        }


        public async Task<Sale?> GetSaleByIdAsync(int id)
        {
            return await _dbContext.Sales
                .IgnoreQueryFilters()
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
                .Include(u => u.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Sale?>  GetSaleForUpdateAsync(int id)
        {
            return await _dbContext.Sales
                .IgnoreQueryFilters()
                .Include(s => s.User)
                .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
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
