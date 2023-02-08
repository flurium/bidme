using Dal.Context;
using Dal.Repository.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dal.Repository
{
    public class LotRepository : BaseRepository<Lot>, ILotRepository
    {
        public LotRepository(BidMeDbContext context) : base(context)
        {
        }

        public async Task<Lot?> FirstOrDefault(Expression<Func<Lot, bool>> conditon) => await Entities.FirstOrDefaultAsync(conditon);

        public virtual async Task DeleteOne(int id) => await base.DeleteOne(l => l.Id == id);

        public async Task Edit(Lot lot)
        {
            Entities.Update(lot);
            await _db.SaveChangesAsync();
        }

        public async Task<Lot?> GetById(int id)
        {
            return await Entities.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}