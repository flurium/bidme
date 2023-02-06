using Domain.Models;

namespace Dal.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task DeleteOne(int Id);

        Task Update(int id, string name);

        Task<Category?> GetById(int id);
    }
}