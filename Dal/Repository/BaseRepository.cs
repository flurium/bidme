using Dal.Context;
using Dal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dal.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _entities;

        protected BidMeDbContext _db;

        protected BaseRepository(BidMeDbContext context)
        {
            _db = context;
            _entities = _db.Set<TEntity>();
        }

        protected DbSet<TEntity> Entities => _entities;

        // Create
        public virtual async Task Create(TEntity entity)
        {
            await Entities.AddAsync(entity).ConfigureAwait(false);
            await _db.SaveChangesAsync();
        }

        public virtual async Task<TEntity?> CreateReturn(TEntity entity)
        {
            var res = await Entities.AddAsync(entity).ConfigureAwait(false);
            if (res == null) return null;
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        // Universal delete
        public virtual async Task DeleteOne(Expression<Func<TEntity, bool>> condition)
        {
            var entity = await Entities.FirstOrDefaultAsync(condition);
            if (entity != null) Entities.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public virtual async Task<TEntity?> DeleteOneReturn(Expression<Func<TEntity, bool>> condition)
        {
            var entity = await Entities.FirstOrDefaultAsync(condition);
            if (entity != null) Entities.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        // Get
        public virtual async Task<IReadOnlyCollection<TEntity>> FindMany(Expression<Func<TEntity, bool>> conditon)
            => await Entities.Where(conditon).ToListAsync().ConfigureAwait(false);

        public virtual async Task<IReadOnlyCollection<TEntity>> FindByConditions(List<Expression<Func<TEntity, bool>>> conditons)
        {
            IQueryable<TEntity> entities = Entities;
            foreach (var conditon in conditons)
            {
                entities = entities.Where(conditon);
            }
            return await entities.ToListAsync().ConfigureAwait(false);
        }

        public async Task<TEntity?> FindOne(Expression<Func<TEntity, bool>> conditon)
            => await Entities.FirstOrDefaultAsync(conditon);

        public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
            => await Entities.ToListAsync().ConfigureAwait(false);

        public async Task<IReadOnlyCollection<TEntity>> OrderDescending(Expression<Func<TEntity, bool>> condition)
        {
            return await Entities.OrderByDescending(condition).ToListAsync().ConfigureAwait(false);
        }
    }
}