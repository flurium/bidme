using System.Linq.Expressions;

namespace Dal.Repository.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<IReadOnlyCollection<TEntity>> GetAllAsync();

        Task<IReadOnlyCollection<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> conditon);

        Task<IReadOnlyCollection<TEntity>> FindByConditions(List<Expression<Func<TEntity, bool>>> conditons);

        Task<IReadOnlyCollection<TEntity>> OrderDescending(Expression<Func<TEntity, bool>> conditon);

        Task CreateAsync(TEntity entity);
    }
}