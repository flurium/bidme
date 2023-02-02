using Domain.Models;

namespace Dal.Repository.Interfaces
{
    public interface IFavoriteRepository : IRepository<Favorite>
    {
        Task Delete(string UserId, int LotId);

        Task<bool> IsFavoriteExistsAsync(Favorite favorite);
    }
}