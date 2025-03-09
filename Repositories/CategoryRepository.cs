using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(TaskManagementDbContext context) : base(context)
        {
        }
    }

}
