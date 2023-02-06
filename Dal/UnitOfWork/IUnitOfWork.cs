using Dal.Repository.Interfaces;

namespace Dal.UnitOfWork
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public IFavoriteRepository FavoriteRepository { get; }
        public ILotImageRepository LotImageRepository { get; }
        public ILotRepository LotRepository { get; }
        public IOrderRepository OrderRepository { get; }
    }
}