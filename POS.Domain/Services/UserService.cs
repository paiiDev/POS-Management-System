using POS.Database.Entities;
using POS.Database.Interfaces;
using POS.Domain.Interfaces;
using POS.Shared.Common;
using POS.Shared.DTOs.Pagination;
using POS.Shared.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<List<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                var userDtos = users.Select(u => new UserDto
                {
                    Id = u.Id,
                   UserName = u.UserName,
                    FullName = u.FullName,
                    PasswordHash = u.PasswordHash,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                }).ToList();

                return Result<List<UserDto>>.Success(userDtos);
            }
            catch (Exception ex)
            {
                return Result<List<UserDto>>.Failure($"An error occurred while retrieving users: {ex.Message}");
            }
        }

        public async Task<Result<PagedResult<UserDto>>> GetPagedUsersAsync(int pageNumber, int pageSize)
        {
          try
            {
                var (users, totalCount) = await _userRepository.GetUsersPagedAsync(pageNumber, pageSize);
                var userDtos = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    PasswordHash = u.PasswordHash,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                }).ToList();

                var pagedResult = new PagedResult<UserDto>
                {
                    Items = userDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                return Result<PagedResult<UserDto>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<UserDto>>.Failure($"An error occurred while retrieving paged users: {ex.Message}");
            }
        }

        public async Task<Result<UserDto?>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return Result<UserDto?>.Failure("User not found.");
                }
                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    PasswordHash = user.PasswordHash,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                };

                return Result<UserDto?>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto?>.Failure($"An error occurred while retrieving the user: {ex.Message}");
            }
        }

        public async Task<Result<bool>> CreateUserAsync(CreateUserDto dto)
        {
            try
            {
                var role = NormalizeRole(dto.Role);
                if (role is null)
                {
                    return Result<bool>.Failure("Please select a valid user role.");
                }

                var hasedPassword = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
                var existingUser = await _userRepository.GetUserByUserNameAsync(dto.UserName.Trim());

                if (existingUser != null)
                {
                    return Result<bool>.Failure("A user with the same username already exists.");
                } 

                    var user = new User
                    {
                        UserName = dto.UserName.Trim(),
                        FullName = dto.FullName.Trim(),
                        PasswordHash = hasedPassword,
                        Role = role,
                        CreatedAt = DateTime.UtcNow.AddHours(6).AddMinutes(30)
                    };

                await _userRepository.CreateUserAsync(user);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"An error occurred while adding the user: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateUserAsync(UpdateUserDto dto)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(dto.Id);
                if (user == null)
                {
                    return Result<bool>.Failure("User not found.");
                }

                var role = NormalizeRole(dto.Role);
                if (role is null)
                {
                    return Result<bool>.Failure("Please select a valid user role.");
                }

                var userName = dto.UserName.Trim();
                var existingUser = await _userRepository.GetUserByUserNameAsync(userName);
                if (existingUser != null && existingUser.Id != dto.Id)
                {
                    return Result<bool>.Failure("A user with the same username already exists.");
                }

                user.UserName = userName;
                user.FullName = dto.FullName.Trim();
                user.Role = role;

                await _userRepository.UpdateUserAsync(user);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"An error occurred while updating the user: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return Result<bool>.Failure("User not found.");
                }

                await _userRepository.DeleteUserAsync(id);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"An error occurred while deleting the user: {ex.Message}");
            }
        }

        private static string? NormalizeRole(string role)
        {
            if (string.Equals(role, SystemUser.AdminRole, StringComparison.OrdinalIgnoreCase))
            {
                return SystemUser.AdminRole;
            }

            if (string.Equals(role, SystemUser.DefaultCashierRole, StringComparison.OrdinalIgnoreCase))
            {
                return SystemUser.DefaultCashierRole;
            }

            return null;
        }
    }
}
