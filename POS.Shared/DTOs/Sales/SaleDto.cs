using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.Sales
{
    public class SaleDto
    {
        public int Id { get; set; }

        public string InvoiceNo { get; set; } = null!;

        public DateTime SaleDate { get; set; }

        public decimal TotalAmount { get; set; }

        public List<SaleItemDto> Items { get; set; } = new();
    }
}
