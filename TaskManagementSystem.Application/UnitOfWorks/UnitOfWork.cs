
using DbContexts;
using Microsoft.EntityFrameworkCore;


namespace Application.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskManagementSystemDbContext _context;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TaskManagementSystemDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new Repository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }
        public async Task<TEntity> GetRepositoryAndSave<TEntity>(TEntity entity) where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                var repository = (Repository<TEntity>)_repositories[typeof(TEntity)];
                return await repository.AddAndSave(entity);
            }

            var newRepository = new Repository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), newRepository);
            return await newRepository.AddAndSave(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
