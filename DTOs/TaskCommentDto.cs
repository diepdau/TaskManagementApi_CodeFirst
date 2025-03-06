using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    public class TaskCommentDto
    {
        [Required(ErrorMessage = "TaskId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TaskId must be greater than 0.")]
        [DefaultValue(1)]
        public int TaskId { get; set; }
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        [DefaultValue(1)]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Content is required.")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Content must be between 1 and 500 characters.")]

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
