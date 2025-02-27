using TaskManagement.Models;
namespace TaskManagement.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(TaskDbContext context) : base(context)
        {
        }
        public Category? GetByName(string name) {
            return _dbSet.FirstOrDefault(c => c.Name == name);
        }
    }
}
