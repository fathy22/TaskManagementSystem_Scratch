using System.Linq.Expressions;

namespace Application.UnitOfWorks
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null);
        Task<TEntity> GetById(int id);
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
    }
}
