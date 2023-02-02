using Dal.Context;
using Dal.Repository.Interfaces;
using Domain.Models;

namespace Dal.Repository
{
    public class LotImageRepository : BaseRepository<LotImage>, ILotImageRepository
    {
        public LotImageRepository(BidMeDbContext context) : base(context)
        {
        }
    }
}