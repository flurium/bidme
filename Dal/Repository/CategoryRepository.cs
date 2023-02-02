using Dal.Context;
using Dal.Repository.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(BidMeDbContext context) : base(context)
        {
        }

        public async Task Delete(int id)
        {
            var category = await Entities.FirstOrDefaultAsync(c => c.Id == id).ConfigureAwait(false);
            if (category != null) Entities.Remove(category);
            await _db.SaveChangesAsync();
        }

        public async Task Update(int id, string name)
        {
            var category = await Entities.FirstOrDefaultAsync(c => c.Id == id).ConfigureAwait(false);
            if (category != null) category.Name = name;
            await _db.SaveChangesAsync();
        }

        public async Task<Category> GetByIdAsync(int id) => await Entities.FirstOrDefaultAsync(c => c.Id == id);
    }
}