using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository.Interfaces
{
  public interface IRepository<TEntity>
  {
    Task<IReadOnlyCollection<TEntity>> GetAllAsync();

    Task<IReadOnlyCollection<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> conditon);

    Task CreateAsync(TEntity entity);
  }
}