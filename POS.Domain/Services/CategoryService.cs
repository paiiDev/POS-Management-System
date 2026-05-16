using POS.Database.Entities;
using POS.Database.Interfaces;
using POS.Domain.Interfaces;
using POS.Shared.Common;
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
        public async Task<Result<List<Category>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync();
                return Result<List<Category>>.Success(categories);
            }
            catch (Exception ex)
            {
                return Result<List<Category>>.Failure(ex.Message);
            }
        }
        public async Task<Result<Category?>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                return Result<Category?>.Success(category);
            }
            catch (Exception ex)
            {
                return Result<Category?>.Failure(ex.Message);
            }
        }
        public async Task<Result<Category>> AddCategoryAsync(Category category)
        {
            try
            {
                var addedCategory = await _categoryRepository.AddCategoryAsync(category);
                return Result<Category>.Success(addedCategory);
            }
            catch (Exception ex)
            {
                return Result<Category>.Failure(ex.Message);
            }
        }
        public async Task<Result<bool>> UpdateCategoryAsync(Category category)
        {
            try
            {
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
