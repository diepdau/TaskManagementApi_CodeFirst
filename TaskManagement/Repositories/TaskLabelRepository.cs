using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public class TaskLabelRepository : GenericRepository<TaskLabel>
    {
        public TaskLabelRepository(TaskDbContext context) : base(context)
        {
        }
        public bool Exists (int taskId, int labelId)
        {
            return _dbSet.Any(tl=>tl.TaskId ==taskId && tl.LabelId ==labelId);
        }
        public bool RemoveLabel(int taskId, int labelId)
        {
            var taskLabel=_dbSet.FirstOrDefault(tl => tl.TaskId == taskId && tl.LabelId == labelId);
            if (taskLabel == null) return false;
            _dbSet.Remove(taskLabel);
            _context.SaveChanges();
            return true;
        }
    }
}
