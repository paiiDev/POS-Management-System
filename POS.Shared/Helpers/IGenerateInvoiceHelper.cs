using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.Helpers
{
    public interface IGenerateInvoiceHelper
    {
        
        Task<string> GenerateInvoiceNumber();
    }
}
