using System.Linq.Expressions;

namespace TaskManagementApi.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        //Task<IEnumerable<T>> GetAll();
        IQueryable<T> GetAll();
        Task<T> GetById(int id);
        System.Threading.Tasks.Task Add(T entity);
        System.Threading.Tasks.Task Update(T entity);
        System.Threading.Tasks.Task DeleteT(T entity);
        System.Threading.Tasks.Task Delete(int id);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        Task<IEnumerable<T>> GetPaged(Func<T, bool>? filter, int page, int pageSize);
    }
}
