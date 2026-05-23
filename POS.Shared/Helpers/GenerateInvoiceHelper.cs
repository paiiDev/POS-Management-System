using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using POS.Database.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.Helpers
{
    public class GenerateInvoiceHelper : IGenerateInvoiceHelper
    {
        private readonly AppDbContext _dbContext;
        public GenerateInvoiceHelper(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GenerateInvoiceNumber()
        {
            var result = new SqlParameter
            {
                ParameterName = "@result",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output
            };

            await _dbContext.Database.ExecuteSqlRawAsync("SET @result = NEXT VALUE FOR dbo.OrderNumbers", result);
            int runningNumber = (int)result.Value;

            string datePart = DateTime.Today.ToString("yyyyMMdd"); // 20260523
            string numberPart = runningNumber.ToString("D4");       // 0001, 0002

            return $"INV-{datePart}-{numberPart}"; // INV-20260523-0001
        }
    }
}
