using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class TaskRepository : GenericRepository<Models.Task>
    {
        public TaskRepository(TaskManagementDbContext context) : base(context)
        {

        }
    }
}