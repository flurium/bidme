using Domain.Models;
using System.Linq.Expressions;

namespace Dal.Repository.Interfaces
{
    public interface ILotRepository : IRepository<Lot>
    {
        Task DeleteOne(int id);

        Task Edit(Lot product);

        Task UpdateStatus(int id, bool isClosed);

        Task<Lot?> GetById(int id);

        Task<Lot?> GetByIdWithImages(int id);

        Task<Lot?> GetByIdWithImagesOrders(int id);

        Task<IReadOnlyCollection<Lot>> FindMany<TKey>(Expression<Func<Lot, bool>> find, Expression<Func<Lot, TKey>> order, bool orderDescending);

        Task<IReadOnlyCollection<Lot>> FindManyWithOrdersImages(List<Expression<Func<Lot, bool>>> conditons);
    }
}