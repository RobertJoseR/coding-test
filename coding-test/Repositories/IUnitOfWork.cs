namespace coding_test.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
