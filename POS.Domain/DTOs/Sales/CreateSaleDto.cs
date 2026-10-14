using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.Sales
{
    public class CreateSaleDto
    {
        [Required(ErrorMessage = "At least one item is required.")]
        [MinLength(1, ErrorMessage = "At least one item is required.")]
        public List<CreateSaleItemDto> Items { get; set; } = new List<CreateSaleItemDto>();

        public int UserId { get; set; }
    }
}
