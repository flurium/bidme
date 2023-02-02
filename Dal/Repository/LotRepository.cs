using Dal.Context;
using Dal.Repository.Interfaces;
using Domain.Models;

namespace Dal.Repository
{
    public class LotRepository : BaseRepository<Lot>, ILotRepository
    {
        public LotRepository(BidMeDbContext context) : base(context)
        {
        }
    }
}