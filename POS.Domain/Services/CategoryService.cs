using POS.Database.Entities;
using POS.Database.Interfaces;
using POS.Domain.Interfaces;
using POS.Shared.Common;
using POS.Shared.DTOs.Category;
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

                if (result == null || !result.Any())
                {
                    return Result<List<CategoryDto>>.Failure("No categories found.");
                }

                var categories = result.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();

                return Result<List<CategoryDto>>.Success(categories);
            }
            catch (Exception ex)
            {
                return Result<List<CategoryDto>>.Failure(ex.Message);
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



        public async Task<Result<bool>> AddCategoryAsync(CategoryDto dto)
        {
            try
            {
                var category = new Category
                {
                    Name = dto.Name,
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
                var category = new Category
                {
                    Id = dto.Id,
                    Name = dto.Name,
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
