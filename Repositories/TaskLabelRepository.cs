using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class TaskLabelRepository : GenericRepository<TaskLabel>
    {
        public TaskLabelRepository(TaskManagementDbContext context) : base(context) { }
        public bool Exists(int taskId, int labelId)
        {
            return _dbSet.Any(tl => tl.TaskId == taskId && tl.LabelId == labelId);
        }
        public bool RemoveLabel(int taskId, int labelId)
        {
            var taskLabel = _dbSet.FirstOrDefault(tl => tl.TaskId == taskId && tl.LabelId == labelId);

            if (taskLabel == null) return false;

            _dbSet.Remove(taskLabel);
            _context.SaveChanges();
            return true;
        }


    }
}
