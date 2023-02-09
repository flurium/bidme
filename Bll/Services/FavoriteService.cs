using Dal.UnitOfWork;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Services
{
    public class FavoriteService
    {
        private readonly IUnitOfWork unitOfWork;

        public FavoriteService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(Favorite favorite)
        {
           await unitOfWork.FavoriteRepository.Create(favorite);
        }
        public async Task DeleteAsync(Favorite favorite)
        {
           await unitOfWork.FavoriteRepository.DeleteOne(favorite.UserId, favorite.LotId);
        }

        public async Task<IReadOnlyCollection<Favorite>> FavoritesAsync(string UserId) => await unitOfWork.FavoriteRepository.FindIncludeLotsAsync(f => f.UserId == UserId);
        public async Task<bool> IsExist(Favorite favorite) => await unitOfWork.FavoriteRepository.IsFavoriteExistsAsync(favorite);
    }
}
