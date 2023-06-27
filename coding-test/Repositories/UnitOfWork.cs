using coding_test.Data;
using Microsoft.EntityFrameworkCore;

namespace coding_test.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BankDBContext dbContext;
        private bool disposed;

        public UnitOfWork(BankDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public void Rollback()
        {
            foreach (var entry in dbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }       
      
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
