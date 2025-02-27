using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get;set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User";
        public virtual ICollection<TaskComment>? TaskComments { get; set; } = new List<TaskComment>();

        public List<Task>? Tasks { get; set; }
    }
}
