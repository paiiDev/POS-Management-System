using POS.Core.DTOs.Auth;
using POS.Shared.Common;
using POS.Shared.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<Result<AuthenticatedUserDto>> LoginAsync(LoginDto dto);
    }
}
