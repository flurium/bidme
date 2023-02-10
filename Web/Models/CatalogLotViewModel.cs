using Domain.Models;

namespace Web.Models
{
    public class CatalogLotViewModel : Lot
    {
        public CatalogLotViewModel(Lot lot, bool isFavorite)
        {
            Id = lot.Id;
            Name = lot.Name;
            Description = lot.Description;
            Price = lot.Price;
            CloseTime = lot.CloseTime;
            IsClosed = lot.IsClosed;
            UserId = lot.UserId;
            User = lot.User;
            CategoryId = lot.CategoryId;
            Category = lot.Category;
            Images = lot.Images;
            Favorites = lot.Favorites;
            Orders = lot.Orders;
            IsFavorite = isFavorite;
        }

        public bool IsFavorite { get; set; }
    }
}