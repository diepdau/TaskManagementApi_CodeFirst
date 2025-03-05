using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs.TaskDto
{
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "IsCompleted is required.")]
        public bool IsCompleted { get; set; } = false;
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "CategoryId is required.")]
        public int CategoryId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
