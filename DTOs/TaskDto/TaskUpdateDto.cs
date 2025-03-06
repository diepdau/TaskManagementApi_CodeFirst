using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.TaskDto
{
    public class TaskUpdateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; } = false;
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        public int? UserId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "CategroryId must be greater than 0.")]
        public int? CategoryId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
