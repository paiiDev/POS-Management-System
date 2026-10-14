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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<(IEnumerable<User> users, int totalCount)> GetUsersPagedAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.Users.AsNoTracking();

            if(!string.IsNullOrEmpty(searchTerm))
            {
                var keywords = searchTerm.Trim();
                query = query.Where(u => u.UserName.Contains(keywords) || u.FullName.Contains(keywords));
            }

            var totalCount = await query.CountAsync();
            var users = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (users, totalCount);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser != null) 
            { 
                existingUser.UserName = user.UserName;
                existingUser.FullName = user.FullName;
                existingUser.Role = user.Role;
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedAt = DateTime.Now.AddHours(6).AddMinutes(30);
                await _context.SaveChangesAsync();
            }
        }
    }
}
