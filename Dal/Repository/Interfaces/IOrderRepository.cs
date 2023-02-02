using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task DeleteAsync(string userId, int productId);

        Task<IReadOnlyCollection<Order>> FindIncludeProductsAsync(Expression<Func<Order, bool>> conditon);

        Task Edit(Order order);

        Task<Order?> GetByIdAsync(string userId, int productId);
    }
}
