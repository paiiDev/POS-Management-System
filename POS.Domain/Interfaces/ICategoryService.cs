using POS.Database.Entities;
using POS.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<List<Category>>> GetAllCategoriesAsync();

        Task<Result<Category?>> GetCategoryByIdAsync(int id);

        Task<Result<Category>> AddCategoryAsync(Category category);

        Task<Result<bool>> UpdateCategoryAsync(Category category);

        Task<Result<bool>> DeleteCategoryAsync(int id);
    }
}
