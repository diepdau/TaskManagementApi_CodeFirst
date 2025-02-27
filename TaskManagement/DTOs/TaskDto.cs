namespace TaskManagement.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; } 
        public string CategoryName { get; set; }
    }
}
