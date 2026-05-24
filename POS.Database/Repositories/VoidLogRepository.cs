using Microsoft.EntityFrameworkCore;
using POS.Database.Context;
using POS.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Database.Repositories
{
    public class VoidLogRepository : IVoidLog
    {
        private readonly AppDbContext _dbContext;
        public VoidLogRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateVoidLogAsync(Entities.VoidLog voidLog)
        {
            _dbContext.VoidLogs.Add(voidLog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Entities.VoidLog?> GetVoidLogBySaleIdAsync(int saleId)
        {
            return await _dbContext.VoidLogs.AsNoTracking().FirstOrDefaultAsync(x => x.SaleId == saleId);
        }
    }
}
