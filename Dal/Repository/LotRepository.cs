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

        public async Task<Lot> CreateAsync(Lot entity)
        {
            Lot res = new Lot();
            await Entities.AddAsync(entity).ConfigureAwait(false);
            res = entity;
            await _db.SaveChangesAsync();
            return res;
        }

        public async Task<Lot> FirstOrDefault(Expression<Func<Lot, bool>> conditon)
        {
            return Entities.FirstOrDefaultAsync(conditon).Result;
        }

        public async Task Delete(int id)
        {
            var product = await Entities.FirstOrDefaultAsync(x => x.Id == id);
            if (product != null) Entities.Remove(product);
            await _db.SaveChangesAsync();
        }

        public async Task<Lot?> DeleteAndReturn(int id)
        {
            var lot = await Entities.FirstOrDefaultAsync(x => x.Id == id);
            if (lot != null) Entities.Remove(lot);
            return lot;
        }

        public virtual async Task<IReadOnlyCollection<Lot>> FindByConditionsAsync(IEnumerable<Expression<Func<Lot, bool>>> conditons, bool includeSeller = true)
        {
            IQueryable<Lot> res = Entities;
            foreach (var conditon in conditons)
            {
                res = res.Where(conditon);
            }

            return await res.ToListAsync().ConfigureAwait(false);
        }

        public async Task Edit(Lot lot)
        {
            Entities.Update(lot);
            await _db.SaveChangesAsync();
        }

        public async Task<Lot?> GetByIdAsync(int id)
        {
            return await Entities.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}