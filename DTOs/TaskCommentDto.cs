using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    public class TaskCommentDto
    {
        [Required(ErrorMessage = "TaskId is required.")]
        public int TaskId { get; set; }
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
