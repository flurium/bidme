using Dal.Context;
using Dal.Repository.Interfaces;
using Domain.Models;

namespace Dal.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BidMeDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserById(string Id) => Entities.FirstOrDefault(u => u.Id == Id);
    }
}