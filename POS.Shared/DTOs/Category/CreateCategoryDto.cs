using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.Category
{
    public class CreateCategoryDto
    {

        [Required(ErrorMessage = "Category Name is required.")]
        public string Name { get; set; } = null!;
    }
}
