namespace TaskManagement.Interfaces
{
    public interface IGenericRepository<T>where T:class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        IEnumerable<T> GetPaged(Func<T, bool>?filter, int page , int pageSize, out int totalItems);
    }
}
