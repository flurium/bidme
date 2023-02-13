using Bll.Models;
using Domain.Models;

namespace Web.Models
{
    public class CatalogViewModel
    {
        public Category? Category { get; set; }

        public IReadOnlyCollection<CatalogLotViewModel> Lots { get; set; }

        public IReadOnlyCollection<Category> Subcategories { get; set; }

        public string Route { get; set; }

        public LotFilter Filter { get; set; }
    }
}