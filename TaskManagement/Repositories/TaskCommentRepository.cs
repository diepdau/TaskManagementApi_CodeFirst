using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public class TaskCommentRepository : GenericRepository<TaskComment>
    {
        public TaskCommentRepository(TaskDbContext context) : base(context)
        {
        }
    }
}
