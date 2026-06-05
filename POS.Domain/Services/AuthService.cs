using Microsoft.AspNetCore.Authentication.Cookies;
using POS.Database.Interfaces;
using POS.Domain.Interfaces;
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

        public async Task<Result<ClaimsPrincipal>> LoginAsync(LoginDto dto)
        {
            try
            {
                if(dto is null)
                {
                    return Result<ClaimsPrincipal>.Failure("Invalid login data.");
                }

                var user = await _authRepository.GetDataByUsernameAsync(dto.Username);
                if(user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                {
                    return Result<ClaimsPrincipal>.Failure("Invalid username or password.");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("FullName", user.FullName),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserId", user.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                return Result<ClaimsPrincipal>.Success(principal);

            } catch(Exception ex)
            {
              return Result<ClaimsPrincipal>.Failure($"An error occurred during login: {ex.Message}");
            }
        }
    }
}
