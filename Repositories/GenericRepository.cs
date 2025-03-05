using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;
using TaskManagementApi.Interfaces;

namespace TaskManagementApi.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly TaskManagementDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(TaskManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll() => await _dbSet.ToListAsync();
        public async Task<T> GetById(int id)=> await _dbSet.FindAsync(id);
        

        public async System.Threading.Tasks.Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Update(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                 _dbSet.Remove(entity);
                await  _context.SaveChangesAsync();
            }
        }

    }

}
