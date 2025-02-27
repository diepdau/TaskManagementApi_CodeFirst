using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public class LabelRepository : GenericRepository<Label>
    {
        public LabelRepository(TaskDbContext context) : base(context)
        {
        }
        public Label? GetByName(string name)
        {
            return _dbSet.FirstOrDefault(c => c.Name == name);
        }


    }
}
