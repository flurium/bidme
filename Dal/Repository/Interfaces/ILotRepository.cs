using Domain.Models;
using System.Linq.Expressions;

namespace Dal.Repository.Interfaces
{
    public interface ILotRepository : IRepository<Lot>
    {
        Task<Lot?> FirstOrDefault(Expression<Func<Lot, bool>> conditon);

        Task DeleteOne(int id);

        Task Edit(Lot product);

        Task<Lot?> GetById(int id);
    }
}