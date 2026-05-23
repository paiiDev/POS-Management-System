using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.Sales
{
    public class CreateSaleDto
    {
        public List<CreateSaleItemDto> Items { get; set; } = new List<CreateSaleItemDto>();
    }
}
