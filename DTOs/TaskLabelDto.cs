using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    public class TaskLabelDto
    {
        [Required(ErrorMessage = "TaskId is required.")]
        public int TaskId { get; set; }
        [Required(ErrorMessage = "LabelId is required.")]
        public int LabelId { get; set; }
    }
}
