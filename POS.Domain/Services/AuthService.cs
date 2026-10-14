using POS.Core.DTOs.Auth;
using POS.Core.Interfaces.IServices;
using POS.Database.Interfaces;
using POS.Shared.Common;
using POS.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POS.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Result<AuthenticatedUserDto>> LoginAsync(LoginDto dto)
        {
            try
            {
                if(dto is null)
                {
                    return Result<AuthenticatedUserDto>.Failure("Invalid login data.");
                }

                var user = await _authRepository.GetUserByUsernameAsync(dto.Username.Trim());
                if(user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                {
                    return Result<AuthenticatedUserDto>.Failure("Invalid username or password.");
                }

                var role = string.Equals(user.Role, SystemUser.AdminRole, StringComparison.OrdinalIgnoreCase)
                    ? SystemUser.AdminRole
                    : user.Role;

                var authUser = new AuthenticatedUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = string.IsNullOrWhiteSpace(user.FullName) ? user.UserName : user.FullName,
                    Role = role
                };

                return Result<AuthenticatedUserDto>.Success(authUser);
            }
            catch (Exception ex)
            {
                return Result<AuthenticatedUserDto>.Failure($"An error occurred during login: {ex.Message}");
            }
        }
    }
}
