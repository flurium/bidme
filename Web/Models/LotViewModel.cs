namespace Web.Models
{
    public class LotViewModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int MinimalBid { get; set; }
        public string Description { get; set; }

        public string CloseTime { get; set; }

        public int? Category { get; set; }
        public string? NewCategory { get; set; }

        public IFormFileCollection Url { get; set; }
    }
}