using Dal.Repository;
using Dal.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.UnitOfWork
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public IFavoriteRepository FavoriteRepository { get; }
        public ILotImageRepository LotImageRepository { get; }
        public ILotRepository LotRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IUserRepository UserRepository { get; }
    }
}