using Domain.Models;
using System.Linq.Expressions;

namespace Dal.Repository.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task DeleteAsync(string userId, int productId);

        Task<IReadOnlyCollection<Order>> FindIncludeProductsAsync(Expression<Func<Order, bool>> conditon);

        Task Edit(Order order);

        Task<Order?> GetByIdAsync(string userId, int productId);

        Task<Order> MaxPrice(int lotid);
    }
}