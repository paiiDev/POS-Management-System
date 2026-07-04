using POS.Shared.Common;
using POS.Shared.DTOs.Pagination;
using POS.Shared.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Interfaces
{
    public interface IProductService
    {
        Task<Result<List<ProductDto>>> GetAllProductsAsync();
        Task<Result<PagedResult<ProductDto>>> GetProductsPagedAsync(string? searchTerm,int pageNumber, int pageSize);
        Task<Result<ProductDto>> GetProductByIdAsync(int id);
        Task<Result<bool>> CreateProductAsync(CreateProductDto createProductDto);
        Task<Result<bool>> UpdateProductAsync(UpdateProductDto updateProductDto);
        Task<Result<bool>> DeleteProductAsync(int id);
    }
}
