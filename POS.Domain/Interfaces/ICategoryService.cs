using POS.Database.Entities;
using POS.Shared.Common;
using POS.Shared.DTOs.Category;
using POS.Shared.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<List<CategoryDto>>> GetAllCategoriesAsync();
        Task<Result<PagedResult<CategoryDto>>> GetAllPagedCategoriesAsync(string? searchTerm, int pageNumber, int pageSize);

        Task<Result<CategoryDto?>> GetCategoryByIdAsync(int id);

        Task<Result<bool>> AddCategoryAsync(CreateCategoryDto dto);

        Task<Result<bool>> UpdateCategoryAsync(UpdateCategoryDto dto);

        Task<Result<bool>> DeleteCategoryAsync(int id);
    }
}
