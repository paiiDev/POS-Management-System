using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.User
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "User ID is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string PasswordHash { get; set; } = null!;

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = null!;

    }
}