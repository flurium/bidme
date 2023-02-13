namespace Bll.Models
{
    public class LotFilter
    {
        public LotFilter(string? name, int? categoryId, double? minPrice, double? maxPrice)
        {
            Name = name;
            CategoryId = categoryId;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }

        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public double? MinPrice { get; set; } = null;
        public double? MaxPrice { get; set; } = null;
    }
}