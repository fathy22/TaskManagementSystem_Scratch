

using DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.UnitOfWorks
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly TaskManagementSystemDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(TaskManagementSystemDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> include = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task<TEntity> AddAndSave(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync(); 
            return entity;
        }
        public async Task Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
