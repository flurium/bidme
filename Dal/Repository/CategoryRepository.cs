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

        public virtual async Task DeleteOne(int id) => await base.DeleteOne(c => c.Id == id);

        public async Task Update(int id, string name)
        {
            var category = await Entities.FirstOrDefaultAsync(c => c.Id == id).ConfigureAwait(false);
            if (category != null) category.Name = name;
            await _db.SaveChangesAsync();
        }

        public async Task<Category?> GetById(int id) => await Entities.Include(c=>c.Lots).FirstOrDefaultAsync(c => c.Id == id);
    }
}