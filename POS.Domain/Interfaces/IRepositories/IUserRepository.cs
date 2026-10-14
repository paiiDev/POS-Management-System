using POS.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Database.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();

        Task<(IEnumerable<User> users, int totalCount)> GetUsersPagedAsync(string? searchTerm, int pageNumber, int pageSize);
        Task<User?> GetUserByIdAsync(int id);
        Task  CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);

        Task<User?> GetUserByUserNameAsync(string userName);
    }
}
