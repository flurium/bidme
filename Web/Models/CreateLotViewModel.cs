using Domain.Models;
using NuGet.Protocol;

namespace Web.Models
{
    public class CreateLotViewModel
    {
        public CreateLotViewModel(Category? category, IReadOnlyCollection<Category> subcategories)
        {
            Category = category;
            Subcategories = subcategories;
        }

        public Category? Category { get; set; }
        public IReadOnlyCollection<Category> Subcategories { get; set; }
    }
}