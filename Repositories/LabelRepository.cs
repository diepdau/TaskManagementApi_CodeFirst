using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class LabelRepository : GenericRepository<Label>
    {
        public LabelRepository(TaskManagementDbContext context) : base(context)
        {
        }
        public Label? GetByName(string name)
        {
            return _dbSet.FirstOrDefault(c => c.Name == name);
        }


    }
}
