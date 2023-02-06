using Domain.Models;

namespace Web.Models
{
    public class LotViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public IFormFileCollection Url { get; set; }
    }
}