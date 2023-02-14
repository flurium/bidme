namespace Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public Category? Parent { get; set; }

        public IReadOnlyCollection<Lot> Lots { get; set; }
        public IReadOnlyCollection<Category> Subcategories { get; set; }
    }
}