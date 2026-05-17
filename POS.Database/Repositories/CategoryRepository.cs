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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GetAllCategories
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _dbContext.Categories.AsNoTracking().ToListAsync();
        }

        //GetById
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        //Create
        public async Task AddCategoryAsync(Category category)
        {
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
        }

        //Update
        public async Task UpdateCategoryAsync(Category category)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync(); 
        }

        //Delete
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
