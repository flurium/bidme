namespace Web.Models
{
    public class LotDetailsViewModel
    {
        public LotDetailsViewModel(int id, string name, IReadOnlyCollection<string> images,
            double startPrice, double currentPrice, DateTime closeTime, bool isClosed, string description)
        {
            Id = id;
            Name = name;
            Images = images;
            StartPrice = startPrice;
            CurrentPrice = currentPrice;
            CloseTime = closeTime;
            IsClosed = isClosed;
            Description = description;
        }

        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public IReadOnlyCollection<string> Images { get; set; }

        public double StartPrice { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime CloseTime { get; set; }
        public bool IsClosed { get; set; }
    }
}