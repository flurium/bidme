using Dal.Context;
using Dal.Repository.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Dal.Repository
{
    public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(BidMeDbContext context) : base(context)
        {
        }

        public async Task DeleteOne(string userId, int lotId) => await base.DeleteOne(f => f.UserId == userId && f.LotId == lotId);

        public async Task<bool> IsFavoriteExistsAsync(Favorite favorite) => await Entities.AnyAsync(f => f.UserId == favorite.UserId && f.LotId == favorite.LotId).ConfigureAwait(false);

        public virtual async Task<IReadOnlyCollection<Favorite>> FindIncludeLotsAsync(Expression<Func<Favorite, bool>> conditon)
              => await Entities.Include(f => f.Lot).Where(conditon).ToListAsync().ConfigureAwait(false);
    }
}