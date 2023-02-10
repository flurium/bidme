using Domain.Models;

namespace Web.Models
{
    public class CatalogViewModel
    {
        public IReadOnlyCollection<CatalogLotViewModel> Lots { get; set; }

        public IReadOnlyCollection<Category> Categories { get; set; }

        public List<string> SelectedCategories { get; set; }

        public string Route { get; set; }
    }
}