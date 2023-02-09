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
            var entity = await GetById(lot.Id);
            if (entity == null) return;

            entity.Name = lot.Name;
            entity.Description = lot.Description;
            entity.Price = lot.Price;
            entity.CloseTime = lot.CloseTime;
            entity.IsClosed = lot.IsClosed;
            entity.UserId = lot.UserId;
            entity.CategoryId = lot.CategoryId;

            await _db.SaveChangesAsync();
        }

        public async Task<Lot?> GetById(int id)
        {
            return await Entities.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Lot?> GetByIdWithImages(int id)
        {
            return await Entities.Include(l => l.Images).FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IReadOnlyCollection<Lot>> FindByConditions(List<Expression<Func<Lot, bool>>> conditons)
        {
            IQueryable<Lot> entities = Entities;
            foreach (var conditon in conditons)
            {
                entities = entities.Where(conditon);
            }
            return await entities.Include(l => l.Images).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<Lot>> FindMany<TKey>(Expression<Func<Lot, bool>> find, Expression<Func<Lot, TKey>> order, bool orderDescending)
        {
            IQueryable<Lot> entities = Entities.Where(find);
            entities = orderDescending ? entities.OrderByDescending(order) : entities.OrderBy(order);
            return await entities.ToListAsync().ConfigureAwait(false);
        }

        public async Task UpdateStatus(int id, bool isClosed)
        {
            var entity = await GetById(id);
            if (entity == null) return;

            entity.IsClosed = isClosed;
            await _db.SaveChangesAsync();
        }

        public async Task<Lot?> GetByIdWithImagesOrders(int id)
        {
            return await Entities.Include(l => l.Images).Include(l => l.Orders).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}