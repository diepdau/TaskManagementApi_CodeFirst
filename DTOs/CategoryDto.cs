using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    [Index(nameof(Name), IsUnique = true)]
    public class CategoryDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(200)]
        public string Description { get; set; }
    }
}
