using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class TaskLabelRepository : GenericRepository<TaskLabel>
    {
        public TaskLabelRepository(TaskManagementDbContext context) : base(context) { }
        public async Task<bool> Exists(int taskId, int labelId)
        {
            return await _dbSet.AnyAsync(tl => tl.TaskId == taskId && tl.LabelId == labelId);
        }
        public async Task<bool> RemoveLabel(int taskId, int labelId)
        {
            var taskLabel = await _dbSet.FirstOrDefaultAsync(tl => tl.TaskId == taskId && tl.LabelId == labelId);

            if (taskLabel == null) return false;

            _dbSet.Remove(taskLabel);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
