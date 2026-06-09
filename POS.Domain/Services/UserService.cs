using POS.Database.Entities;
using POS.Database.Interfaces;
using POS.Domain.Interfaces;
using POS.Shared.Common;
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

                var hasedPassword = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

                var user = new User
                {
                    UserName = dto.UserName.Trim(),
                    FullName = dto.FullName.Trim(),
                    PasswordHash = hasedPassword,
                    Role = dto.Role,
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

                user.UserName = dto.UserName;
                user.FullName = dto.FullName;
                user.Role = dto.Role;

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
    }
}