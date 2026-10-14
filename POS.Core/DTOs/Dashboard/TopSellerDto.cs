using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.Dashboard
{
    public class TopSellerDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int UnitsSold { get; set; }
        public string Initial => string.IsNullOrEmpty(ProductName) ? "" : ProductName.Substring(0, 1).ToUpper();
    }
}
