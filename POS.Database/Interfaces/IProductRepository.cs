using POS.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Database.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<(IEnumerable<Product> products, int totalCount)> GetProductsPagedAsync(string? searchTerm,int pageNumber, int pageSize);
        Task<Entities.Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsforCreateSale(List<int> productIds);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
