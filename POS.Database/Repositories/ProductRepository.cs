using Microsoft.EntityFrameworkCore;
using POS.Database.Context;
using POS.Database.Entities;
using POS.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Database.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products.Include(p => p.Category).AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();
        }


        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _dbContext.Products.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<Product>> GetProductsforCreateSale(List<int> productIds)
        {
            return await _dbContext.Products.Where(x => productIds.Contains(x.Id) && !x.IsDeleted).ToListAsync();
        }


        public async Task CreateProductAsync(Product product)
        {
             _dbContext.Products.Add(product);
             await _dbContext.SaveChangesAsync();
        }


        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == product.Id && !x.IsDeleted);
            if (existingProduct != null)
            {

                existingProduct.Id = product.Id;
                existingProduct.Name = product.Name;
                existingProduct.Barcode = product.Barcode;
                existingProduct.CostPrice = product.CostPrice;
                existingProduct.SellingPrice = product.SellingPrice;
                existingProduct.StockQuantity = product.StockQuantity;
                existingProduct.CategoryId = product.CategoryId;
                await _dbContext.SaveChangesAsync();

            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (product != null)
            {
                product.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
