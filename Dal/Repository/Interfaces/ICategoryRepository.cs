using Domain.Models;

namespace Dal.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task Delete(int Id);

        Task Update(int id, string name);

        Task<Category> GetByIdAsync(int id);
    }
}