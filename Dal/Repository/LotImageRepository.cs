using Dal.Context;
using Dal.Repository.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Repository
{
    public class LotImageRepository : BaseRepository<LotImage>, ILotImageRepository
    {
        public LotImageRepository(BidMeDbContext context) : base(context)
        {
        }

    }
}