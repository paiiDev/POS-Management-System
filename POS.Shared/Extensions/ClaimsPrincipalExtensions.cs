using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetFullName(this ClaimsPrincipal principal)
        {
            var claim = principal.Claims.FirstOrDefault(x => x.Type == "FullName");
            return claim != null ? claim.Value : "Unknown user";
        }
    }
}
