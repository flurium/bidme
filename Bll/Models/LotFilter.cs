namespace Bll.Models
{
    public class LotFilter
    {
        public string? Name { get; set; }
        public List<string> Categories { get; set; }

        public double? MinPrice { get; set; } = null;
        public double? MaxPrice { get; set; } = null;
    }
}