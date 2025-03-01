

namespace TaskManagementApi.Models
{
    public class TaskLabel
    {
        public int TaskId { get; set; }
        public Task? Tasks { get; set; }
        public int LabelId { get; set; }
        public Label? Labels { get; set; }
    }
}
