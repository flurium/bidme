namespace Bll.Models
{
    public class LotFilter
    {
        public LotFilter(string? name, List<string> categories, double? minPrice, double? maxPrice)
        {
            Name = name;
            Categories = categories;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }

        public string? Name { get; set; }
        public List<string> Categories { get; set; }
        public double? MinPrice { get; set; } = null;
        public double? MaxPrice { get; set; } = null;
    }
}