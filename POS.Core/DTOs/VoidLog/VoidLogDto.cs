using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.VoidLog
{
    public class VoidLogDto
    {
        public int SaleId { get; set; }

        public string Reason { get; set; } = null!;


    }
}
