using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
