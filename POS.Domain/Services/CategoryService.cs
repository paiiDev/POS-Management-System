using POS.Database.Entities;
using POS.Database.Interfaces;
using POS.Domain.Interfaces;
using POS.Shared.Common;
using POS.Shared.DTOs.Category;
using POS.Shared.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<List<CategoryDto>>> GetAllCategoriesAsync()
        {
            try
            {
                var result = await _categoryRepository.GetAllCategoriesAsync();

                if (result == null)
                {
                    return Result<List<CategoryDto>>.Failure("No categories found.");
                }

                var categories = result.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name ?? "No category",
                }).ToList();

                return Result<List<CategoryDto>>.Success(categories);
            }
            catch (Exception ex)
            {
                return Result<List<CategoryDto>>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagedResult<CategoryDto>>> GetAllPagedCategoriesAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            try
            {
                var result = await _categoryRepository.GetAllPagedCategoriesAsync(searchTerm, pageNumber, pageSize);
                if (result.Items == null)
                {
                    return Result<PagedResult<CategoryDto>>.Failure("No categories found.");
                }

                var dto = result.Items.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();

                var pagedResult = new PagedResult<CategoryDto>
                {
                    Items = dto,
                    TotalCount = result.TotalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return Result<PagedResult<CategoryDto>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<CategoryDto>>.Failure(ex.Message);
            }
        }



        public async Task<Result<CategoryDto?>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var result = await _categoryRepository.GetCategoryByIdAsync(id);
                if(result == null )
                {
                    return Result<CategoryDto?>.Failure("Category not found.");
                }
                var category = new CategoryDto
                {
                    Id = result.Id,
                    Name = result.Name,
                };
                return Result<CategoryDto?>.Success(category);
            }
            catch (Exception ex)
            {
                return Result<CategoryDto?>.Failure(ex.Message);
            }
        }



        public async Task<Result<bool>> AddCategoryAsync(CreateCategoryDto dto)
        {
            try
            {
                if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
                {
                    return Result<bool>.Failure("Category name is required.");
                }

                var categoryName = dto.Name.Trim();
                var categories = await _categoryRepository.GetAllCategoriesAsync();
                if (categories.Any(c => string.Equals(c.Name.Trim(), categoryName, StringComparison.OrdinalIgnoreCase)))
                {
                    return Result<bool>.Failure("A category with the same name already exists.");
                }

                var category = new Category
                {
                    Name = categoryName,
                };

                await _categoryRepository.AddCategoryAsync(category);
               
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }



        public async Task<Result<bool>> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            try
            {
                if (dto is null)
                {
                    return Result<bool>.Failure("Category data is required.");
                }

                if (dto.Id <= 0)
                {
                    return Result<bool>.Failure("Invalid category id.");
                }

                if (string.IsNullOrWhiteSpace(dto.Name))
                {
                    return Result<bool>.Failure("Category name is required.");
                }

                var existingCategory = await _categoryRepository.GetCategoryByIdAsync(dto.Id);
                if (existingCategory is null)
                {
                    return Result<bool>.Failure("Category not found.");
                }

                var categoryName = dto.Name.Trim();
                var categories = await _categoryRepository.GetAllCategoriesAsync();
                if (categories.Any(c => c.Id != dto.Id && string.Equals(c.Name.Trim(), categoryName, StringComparison.OrdinalIgnoreCase)))
                {
                    return Result<bool>.Failure("A category with the same name already exists.");
                }

                var category = new Category
                {
                    Id = dto.Id,
                    Name = categoryName,
                };

                 await _categoryRepository.UpdateCategoryAsync(category);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }



        public async Task<Result<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return Result<bool>.Failure("Invalid category id.");
                }

                var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);
                if (existingCategory is null)
                {
                    return Result<bool>.Failure("Category not found.");
                }

                await _categoryRepository.DeleteCategoryAsync(id);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }
    }
}
