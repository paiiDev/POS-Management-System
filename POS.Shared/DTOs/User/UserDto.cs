using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string Role { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
