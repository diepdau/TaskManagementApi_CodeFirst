using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(TaskManagementDbContext context) : base(context)
        {
        }
        public Category? GetByName(string name)
        {
            return _dbSet.FirstOrDefault(c => c.Name == name);
        }
        //public IEnumerable<Category> GetAllWithTasks()
        //{
        //    return _context.Categories.Include(c => c.Tasks).ToList();
        //}


    }

}
