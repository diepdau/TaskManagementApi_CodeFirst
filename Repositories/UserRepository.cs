using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(TaskManagementDbContext context) : base(context)
        {
        }
        public User? GetByUsername(string username)
        {
            return _dbSet.FirstOrDefault(u => u.UserName == username);
        }
        public User? GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(u => u.Email == email);
        }
        
    }
}
