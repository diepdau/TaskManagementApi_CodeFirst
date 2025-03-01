

namespace TaskManagementApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Task>? Tasks { get; set; }
    }
}
