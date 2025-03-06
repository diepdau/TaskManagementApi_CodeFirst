using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.DTOs
{
    public class TaskLabelDto
    {
        [Required(ErrorMessage = "TaskId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "TaskId must be greater than 0.")]
        [DefaultValue(1)]
        public int TaskId { get; set; }
        [Required(ErrorMessage = "LabelId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "LabelId must be greater than 0.")]
        [DefaultValue(1)]
        public int LabelId { get; set; }
    }
}
