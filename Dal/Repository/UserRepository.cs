using Dal.Context;
using Dal.Repository.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BidMeDbContext context) : base(context)
        {
        }

        public async Task<User?> GetById(string Id) => await Entities.FirstOrDefaultAsync(u => u.Id == Id);
    }
}