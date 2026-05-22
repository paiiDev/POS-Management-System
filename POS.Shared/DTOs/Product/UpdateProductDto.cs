using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.Product
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ("Product Name is required."))]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Barcode is required.")]
        [StringLength(30, ErrorMessage = "Barcode cannot exceed 30 characters.")]
        public string Barcode { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Cost Price must be greater than 0")]
        public decimal CostPrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Selling Price must be greater than 0")]
        public decimal SellingPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int StockQuantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
    }
}
