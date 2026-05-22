using POS.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.Helpers
{
    public static class ProductValidationHelper
    {
        public static Result<bool>? ValidateProduct(string name, string barcode, decimal costPrice, decimal sellingPrice, int stockQuantity, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<bool>.Failure("Product name cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(barcode))
            {
                return Result<bool>.Failure("Product barcode cannot be empty.");
            }
            if (costPrice < 0)
            {
                return Result<bool>.Failure("Product cost price cannot be negative.");
            }
            if (sellingPrice < 0)
            {
                return Result<bool>.Failure("Product selling price cannot be negative.");
            }
            if (stockQuantity < 0)
            {
                return Result<bool>.Failure("Stock quantity cannot be negative.");
            }
            if (categoryId <= 0)
            {
                return Result<bool>.Failure("Invalid product category.");
            }

            return null;
        }
    }
}
