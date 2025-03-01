using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    public class CategoryDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
