namespace TaskManagement.DTOs
{
    public class TaskCommentDto
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; }
    }
}
