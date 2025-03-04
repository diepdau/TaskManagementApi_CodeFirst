using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class TaskAttachmentRepository : ITaskAttachmentRepository
    {
        private readonly TaskManagementDbContext _context;
        protected readonly DbSet<TaskAttachment> _dbSet;
        public TaskAttachmentRepository(TaskManagementDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TaskAttachment>();
        }
        public async System.Threading.Tasks.Task AddAttachment(TaskAttachment attachment)
        {
            await _dbSet.AddAsync(attachment);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteAttachment(int id)
        {
            var attachment = await _dbSet.FindAsync(id);
            if (attachment != null)
            {
                 _dbSet.Remove(attachment);
                await _context.SaveChangesAsync();

            }
        }
        public async Task<IEnumerable<TaskAttachment>> GetAttachmentsByTaskId(int taskId)
        {
            return await _dbSet.Where(a => a.TaskId == taskId).ToListAsync();
        }



    }
}