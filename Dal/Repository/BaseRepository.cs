using Dal.Context;
using Dal.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

    public virtual async Task CreateAsync(TEntity entity)
    {
      await Entities.AddAsync(entity).ConfigureAwait(false);
      await _db.SaveChangesAsync();
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> conditon)
        => await Entities.Where(conditon).ToListAsync().ConfigureAwait(false);

    public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
        => await Entities.ToListAsync().ConfigureAwait(false);
  }
}