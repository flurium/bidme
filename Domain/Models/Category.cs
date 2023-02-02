namespace Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Lot> Lots { get; set; }
        
    }
}