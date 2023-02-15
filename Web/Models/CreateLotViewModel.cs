using Domain.Models;

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