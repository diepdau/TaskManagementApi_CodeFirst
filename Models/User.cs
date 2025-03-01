using Microsoft.AspNetCore.Identity;
namespace TaskManagementApi.Models
{
    public class User  : IdentityUser<int>
    {
        public virtual ICollection<TaskComment>? TaskComments { get; set; } = new List<TaskComment>();

        public List<Task>? Tasks { get; set; }
    }
}

