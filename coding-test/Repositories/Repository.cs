using coding_test.Data;
using Microsoft.EntityFrameworkCore;

namespace coding_test.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BankDBContext dbContext;
        private readonly DbSet<T> dbSet;

        public T GetById(object id)
        {
            return dbSet.Find(id);
        }

        public Repository(BankDBContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = this.dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return dbSet.Select(s => s);
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
