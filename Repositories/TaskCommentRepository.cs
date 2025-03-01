using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class TaskCommentRepository : GenericRepository<TaskComment>
    {
        public TaskCommentRepository(TaskManagementDbContext context) : base(context)
        {
        }
    }
}
