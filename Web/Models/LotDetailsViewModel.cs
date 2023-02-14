namespace Web.Models
{
    public class LotDetailsViewModel
    {
        public LotDetailsViewModel(int id, string name, IReadOnlyCollection<string> images,
            double startPrice, double currentPrice, int minimalBid, DateTime closeTime, bool isClosed, string description, string route, bool isFavorite)
        {
            Id = id;
            Name = name;
            Images = images;
            StartPrice = startPrice;
            CurrentPrice = currentPrice;
            MinimalBid = minimalBid;
            CloseTime = closeTime;
            IsClosed = isClosed;
            Description = description;
            Route = route;
            IsFavorite = isFavorite;
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public IReadOnlyCollection<string> Images { get; set; }

        public double StartPrice { get; set; }
        public double CurrentPrice { get; set; }
        public int MinimalBid { get; set; }
        public DateTime CloseTime { get; set; }
        public bool IsClosed { get; set; }

        public string Route { get; set; }
        public bool IsFavorite { get; set; }
    }
}