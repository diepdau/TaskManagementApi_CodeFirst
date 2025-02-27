using TaskManagement.Models;

namespace TaskManagement.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(TaskDbContext context) : base(context)
        {
        }
        public User? GetByUsername(string username)
        {
            return _dbSet.FirstOrDefault(u => u.Username == username);
         }

        public User? GetByEmail(string email) => _dbSet.FirstOrDefault(e => e.Email == email);

    }
}
