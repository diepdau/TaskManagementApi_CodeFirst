
namespace TaskManagementApi.Models
{
    public class TaskComment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public Task? Task { get; set; }
        public int? UserId { get; set; } = null!;
        public User? User { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}