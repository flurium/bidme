using Domain.Models;
using System.Linq.Expressions;

namespace Dal.Repository.Interfaces
{
    public interface IFavoriteRepository : IRepository<Favorite>
    {
        Task DeleteOne(string UserId, int LotId);

        Task<IReadOnlyCollection<Favorite>> FindIncludeLotsAsync(Expression<Func<Favorite, bool>> conditon);

        Task<bool> IsFavoriteExistsAsync(Favorite favorite);
    }
}