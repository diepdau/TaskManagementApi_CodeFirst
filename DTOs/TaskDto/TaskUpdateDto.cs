using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.TaskDto
{
    public class TaskUpdateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; } = false;

        public int? UserId { get; set; }

        public int? CategoryId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
