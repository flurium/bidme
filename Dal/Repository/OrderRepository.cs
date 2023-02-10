using Dal.Context;
using Dal.Repository.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dal.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(BidMeDbContext context) : base(context)
        {
        }

        public async Task DeleteAsync(string userId, int lotId)
        {
            var order = await Entities.FirstOrDefaultAsync(o => o.UserId == userId && o.LotId == lotId).ConfigureAwait(false);
            if (order != null) Entities.Remove(order);
            await _db.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Order>> FindIncludeProductsAsync(Expression<Func<Order, bool>> conditon)
            => await Entities.Include(o => o.Lot).Where(conditon).ToListAsync().ConfigureAwait(false);

        public async Task Edit(Order order)
        {
            var entity = Entities.FirstOrDefault(o => o.LotId == order.LotId && o.UserId == order.UserId);
            if (entity == null) return;

            entity.OrderPrice = order.OrderPrice;
            Entities.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<Order?> GetById(string userId, int lotId) => await Entities.FirstOrDefaultAsync(o => o.UserId == userId && o.LotId == lotId);

        public async Task<Order?> MaxPrice(int lotid)
        {
            var order = await Entities.Where(x => x.LotId == lotid).OrderByDescending(n => n).FirstOrDefaultAsync();

            return order;
        }
    }
}