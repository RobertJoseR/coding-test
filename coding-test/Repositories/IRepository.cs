namespace coding_test.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetById(object id);
        IQueryable<T> GetAll();
        void Add(T entity);

        void Remove(T entity);
    }
}
