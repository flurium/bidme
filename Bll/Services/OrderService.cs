using Dal.UnitOfWork;
using Domain.Models;
using System.Linq.Expressions;

namespace Bll.Services
{
    public class OrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyCollection<Order>> UserOrders(string uid, bool isClosed)
        {
            return await _unitOfWork.OrderRepository.FindIncludeProductsAsync(o => o.UserId == uid && o.Lot.IsClosed == isClosed);
        }
    }
}