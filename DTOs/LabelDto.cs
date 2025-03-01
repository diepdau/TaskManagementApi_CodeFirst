using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    public class LabelDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
