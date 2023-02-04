using Dal.Repository.Interfaces;

namespace Dal.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IOrderRepository orderRepository, ILotImageRepository lotImageRepository,
            ICategoryRepository categoryRepository, IFavoriteRepository favoriteRepository,
            ILotRepository lotRepository, IUserRepository userRepository)
        {
            OrderRepository = orderRepository;
            LotImageRepository = lotImageRepository;
            CategoryRepository = categoryRepository;
            FavoriteRepository = favoriteRepository;
            LotRepository = lotRepository;
            UserRepository = userRepository;
        }

        public IOrderRepository OrderRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public IFavoriteRepository FavoriteRepository { get; }

        public ILotImageRepository LotImageRepository { get; }

        public ILotRepository LotRepository { get; }

        public IUserRepository UserRepository { get; }
    }
}