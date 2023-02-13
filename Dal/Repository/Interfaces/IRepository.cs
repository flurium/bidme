using System.Linq.Expressions;

namespace Dal.Repository.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<IReadOnlyCollection<TEntity>> GetAllAsync();

        Task<IReadOnlyCollection<TEntity>> FindMany(Expression<Func<TEntity, bool>> conditon);

        Task<TEntity?> FindOne(Expression<Func<TEntity, bool>> conditon);

        Task<IReadOnlyCollection<TEntity>> FindByConditions(List<Expression<Func<TEntity, bool>>> conditons);

        Task<IReadOnlyCollection<TEntity>> OrderDescending(Expression<Func<TEntity, bool>> conditon);

        // Create
        Task Create(TEntity entity);

        Task<TEntity?> CreateReturn(TEntity entity);

        /// <summary> Universal delete </summary>
        Task DeleteOne(Expression<Func<TEntity, bool>> condition);

        /// <summary> Universal delete and return back</summary>
        /// <returns>Entity or null if entity wasn't found</returns>
        Task<TEntity?> DeleteOneReturn(Expression<Func<TEntity, bool>> condition);
    }
}