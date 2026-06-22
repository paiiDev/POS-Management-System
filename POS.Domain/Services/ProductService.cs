using POS.Database.Entities;
using POS.Database.Interfaces;
using POS.Domain.Interfaces;
using POS.Shared.Common;
using POS.Shared.DTOs.Pagination;
using POS.Shared.DTOs.Product;
using POS.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }


        public async Task<Result<List<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                var result = await _productRepository.GetAllProductsAsync();

                var products = result.Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Barcode = x.Barcode,
                    CostPrice = x.CostPrice,
                    SellingPrice = x.SellingPrice,
                    StockQuantity = x.StockQuantity,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name
                }).ToList();

                return Result<List<ProductDto>>.Success(products);
            }
            catch (Exception ex)
            {
                return Result<List<ProductDto>>.Failure(ex.Message);
            }
        }



        public async Task<Result<PagedResult<ProductDto>>> GetProductsPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                var (products, totalCount) = await _productRepository.GetProductsPagedAsync(pageNumber, pageSize);

              

                var dtoList = products.Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Barcode = x.Barcode,
                    CostPrice = x.CostPrice,
                    SellingPrice = x.SellingPrice,
                    StockQuantity = x.StockQuantity,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name
                }).ToList();

                  var result =  new PagedResult<ProductDto>
                {
                    Items = dtoList,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                return Result<PagedResult<ProductDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<ProductDto>>.Failure(ex.Message);
            }
        }


        public async Task<Result<ProductDto>> GetProductByIdAsync(int id)
        {
            try
            {
                var result = await _productRepository.GetProductByIdAsync(id);
                if (result is null)
                {
                    return Result<ProductDto>.Failure("Product not found,");
                }

                var product = new ProductDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    Barcode = result.Barcode,
                    CostPrice = result.CostPrice,
                    SellingPrice = result.SellingPrice,
                    StockQuantity = result.StockQuantity,
                    CategoryId = result.CategoryId,
                    CategoryName = result.Category.Name
                };

                return Result<ProductDto>.Success(product);
            }
            catch (Exception ex)
            {
                return Result<ProductDto>.Failure(ex.Message);
            }
        }


        public async Task<Result<bool>> CreateProductAsync(CreateProductDto dto)
        {
            try
            {
                if( dto is null)
                {
                    return Result<bool>.Failure("Product data is required.");
                }

                var validationResult = ProductValidationHelper.ValidateProduct(dto.Name, dto.Barcode, dto.CostPrice, dto.SellingPrice, dto.StockQuantity, dto.CategoryId);
                if(validationResult is not null && !validationResult.IsSuccess)
                {
                    return validationResult;
                }

                var productName = dto.Name.Trim();
                var barcode = dto.Barcode.Trim();

                var isCategoryIdExist = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
                if (isCategoryIdExist is null)
                {
                    return Result<bool>.Failure("Product category not found.");
                }

                var products = await _productRepository.GetAllProductsAsync();
                var existingBarCode = products.Any(x => x.Barcode.Trim() == barcode);
                if(existingBarCode)
                {
                    return Result<bool>.Failure("Product's barcode must be unique.");
                }

                var product = new Product
                {
                    Name = productName,
                    Barcode = barcode,
                    CostPrice = dto.CostPrice,
                    SellingPrice = dto.SellingPrice,
                    StockQuantity = dto.StockQuantity,
                    CategoryId = dto.CategoryId,

                };

                await _productRepository.CreateProductAsync(product);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }


        public async Task<Result<bool>> UpdateProductAsync(UpdateProductDto dto)
        {
            try
            {
                if(dto is null)
                {
                    return Result<bool>.Failure("Product data is required.");
                }
                if (dto.Id <= 0)
                {
                    return Result<bool>.Failure("Invalid product id");
                }

                var validationResult = ProductValidationHelper.ValidateProduct(dto.Name, dto.Barcode, dto.CostPrice, dto.SellingPrice, dto.StockQuantity, dto.CategoryId);
                if(validationResult is not null && !validationResult.IsSuccess)
                {
                    return validationResult;
                }

                var existingProduct = await _productRepository.GetProductByIdAsync(dto.Id);
                if (existingProduct is null)
                {
                    return Result<bool>.Failure("Product not found.");
                }

                var productName = dto.Name.Trim();
                var barcode = dto.Barcode.Trim();

                var isCategoryIdExist = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
                if (isCategoryIdExist is null)
                {
                    return Result<bool>.Failure("Product category not found.");
                }

                var products = await _productRepository.GetAllProductsAsync();
                var existingBarCode = products.Any(x => x.Barcode.Trim() == barcode && x.Id != dto.Id );
                if (existingBarCode)
                {
                    return Result<bool>.Failure("Product barcode must be unique.");
                }

                var product = new Product
                {
                    Id = dto.Id,
                    Name = productName,
                    Barcode = barcode,
                    CostPrice = dto.CostPrice,
                    SellingPrice = dto.SellingPrice,
                    StockQuantity = dto.StockQuantity,
                    CategoryId = dto.CategoryId,
                };

                await _productRepository.UpdateProductAsync(product);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }


        public async Task<Result<bool>> DeleteProductAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Result<bool>.Failure("Invalid product id.");
                }

                var existingProduct = await _productRepository.GetProductByIdAsync(id);
                if (existingProduct is null)
                {
                    return Result<bool>.Failure("Product not found.");
                }

                await _productRepository.DeleteProductAsync(id);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }
    }
}
