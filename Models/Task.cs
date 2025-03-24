

namespace TaskManagementApi.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<TaskComment>? TaskComments { get; set; }
        public List<TaskLabel>? TaskLabels { get; set; } = new();
        public List<TaskAttachment>? TaskAttachments { get; set; }
    }
}
