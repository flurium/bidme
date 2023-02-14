namespace Domain.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public double Price { get; set; }
        public int MinimalBid { get; set; }

        public DateTime CloseTime { get; set; }

        // for less calculation and additional safety
        public bool IsClosed { get; set; } = false;

        public string? UserId { get; set; }
        public User? User { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Navigation
        public IReadOnlyCollection<LotImage> Images { get; set; }

        public IReadOnlyCollection<Favorite> Favorites { get; set; }

        public IReadOnlyCollection<Order> Orders { get; set; }
    }
}