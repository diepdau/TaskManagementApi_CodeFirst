using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(TaskManagementDbContext context) : base(context)
        {
        }
      
        
    }
}
