namespace TaskManagementApi.Models
{
    public class TaskAttachment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public Task? Task { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public DateTime UploadedAt { get; set; }
       
    }
}
