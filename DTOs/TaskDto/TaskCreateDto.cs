using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TaskManagementApi.Models;

namespace TaskManagementApi.DTOs.TaskDto
{
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        //[Required(ErrorMessage = "IsCompleted is required.")]
        public bool IsCompleted { get; set; } = false;
        [Required(ErrorMessage = "UserId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
        [DefaultValue(1)]
        public int UserId { get; set; }
        [Required(ErrorMessage = "CategoryId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0.")]
        [DefaultValue(1)]
        public int CategoryId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public string? CategoryName { get; set; } 
        public string? UserName { get; set; }
        public string? LabelName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<string>? Labels { get; set; }
    }
}
