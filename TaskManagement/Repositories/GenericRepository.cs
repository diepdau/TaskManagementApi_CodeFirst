using TaskManagement.Interfaces;
using TaskManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TaskManagement.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly TaskDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(TaskDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public T GetById(int id) => _dbSet.Find(id);

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        } 
        public IEnumerable<T> GetPaged(Func<T, bool>? filter, int page, int pageSize, out int totalItems)
        {
            var query = _dbSet.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter).AsQueryable();
            }

            totalItems = query.Count();

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }

}
