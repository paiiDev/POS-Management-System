using POS.Shared.Common;
using POS.Shared.DTOs.Category;
using POS.Shared.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Interfaces
{
    public interface IUserService
    {
        
        Task<Result<List<UserDto>>> GetAllUsersAsync();
        Task<Result<UserDto?>> GetUserByIdAsync(int id);
        Task<Result<bool>> AddUserAsync(CreateUserDto dto);
        Task<Result<bool>> UpdateUserAsync(UpdateUserDto dto);
        Task<Result<bool>> DeleteUserAsync(int id);
    }
}
