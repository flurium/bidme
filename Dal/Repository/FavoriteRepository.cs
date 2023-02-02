using Microsoft.EntityFrameworkCore;
using Dal.Context;
using Domain.Models;
using Dal.Repository.Interfaces;

namespace Dal.Repository
{
    public class FavoriteRepository : BaseRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(BidMeDbContext context) : base(context)
        {
        }

        public async Task Delete(string UserId, int ProductId)
        {
            var favorite = await Entities.FirstOrDefaultAsync(o => o.UserId == UserId && o.LotId == ProductId).ConfigureAwait(false);
            if (favorite != null) Entities.Remove(favorite);
            _db.SaveChanges();
        }

        public async Task<bool> IsFavoriteExistsAsync(Favorite favorite) => await Entities.AnyAsync(f => f.UserId == favorite.UserId && f.LotId == favorite.LotId).ConfigureAwait(false);
    }
}