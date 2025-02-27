using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public class TaskRepository : GenericRepository<Models.Task>
    {
        public TaskRepository(TaskDbContext context) : base(context)
        {
        }
        
    }
}
