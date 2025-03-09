using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class LabelRepository : GenericRepository<Label>
    {
        public LabelRepository(TaskManagementDbContext context) : base(context)
        {
        }
    }
}
