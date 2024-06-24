

namespace Application.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        void Save();
        Task<TEntity> GetRepositoryAndSave<TEntity>(TEntity entity) where TEntity : class;
    }
}
