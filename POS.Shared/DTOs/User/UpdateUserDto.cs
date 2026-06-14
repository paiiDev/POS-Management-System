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
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9@\.\-_]+$", ErrorMessage = "Username can only contain letters, numbers, and symbols like @, ., -, _ (No spaces allowed).")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name can only contain letters and spaces.")]
        public string FullName { get; set; } = null!;


        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; } = null!;

    }
}
