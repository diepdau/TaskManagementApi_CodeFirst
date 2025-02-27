using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class Label
    {
        public int Id { get;set; }
        public string Name { get; set; }
        public List<TaskLabel> TaskLabels { get; set; }
    }
}
