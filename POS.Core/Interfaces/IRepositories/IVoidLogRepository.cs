using POS.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Database.Interfaces
{
    public interface IVoidLogRepository
    {
        Task CreateVoidLogAsync(Entities.VoidLog voidLog);

        Task<VoidLog?> GetVoidLogBySaleIdAsync(int saleId);

    }
}
