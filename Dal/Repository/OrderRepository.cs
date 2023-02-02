using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository
{
    public class OrderRepository
    {
        public OrderRepository(DbContext context) : base(context)
        {
        }

        public async Task DeleteAsync(string userId, int lotId)
        {
            var order = await Entities.FirstOrDefaultAsync(o => o.UserId == userId && o.LotId == lotId).ConfigureAwait(false);
            if (order != null) Entities.Remove(order);
            await _db.SaveChangesAsync();
        }

        public virtual async Task<IReadOnlyCollection<Order>> FindIncludeProductsAsync(Expression<Func<Order, bool>> conditon)
            => await Entities.Include(o => o.Lot).Where(conditon).ToListAsync().ConfigureAwait(false);

        public async Task Edit(Order order)
        {
            Entities.Update(order);
            await _db.SaveChangesAsync();
        }

        public async Task<Order?> GetByIdAsync(string userId, int lotId)
        {
            return await Entities.FirstOrDefaultAsync(o => o.UserId == userId && o.LotId == lotId);
        }       
    }
}
