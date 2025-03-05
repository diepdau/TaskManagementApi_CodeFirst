namespace TaskManagementApi.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        System.Threading.Tasks.Task Add(T entity);
        System.Threading.Tasks.Task Update(T entity);
        System.Threading.Tasks.Task Delete(int id);
    }
}
