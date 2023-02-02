using Domain.Models;

namespace Dal.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserById(string Id);
    }
}