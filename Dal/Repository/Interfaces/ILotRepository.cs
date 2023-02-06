using Domain.Models;
using System.Linq.Expressions;

namespace Dal.Repository.Interfaces
{
    public interface ILotRepository : IRepository<Lot>
    {
        Task<Lot> CreateAsync(Lot entity);

        Task<Lot> FirstOrDefault(Expression<Func<Lot, bool>> conditon);

        Task Delete(int id);

        Task<Lot?> DeleteAndReturn(int id);

        Task<IReadOnlyCollection<Lot>> FindByConditionsAsync(IEnumerable<Expression<Func<Lot, bool>>> conditons, bool includeSeller = true);

        Task Edit(Lot product);

        Task<Lot?> GetByIdAsync(int id);
    }
}