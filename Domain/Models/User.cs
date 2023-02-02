using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class User : IdentityUser
    {
        public double SellerRating { get; set; }
        public double BuyerRating { get; set; }

        public ICollection<Lot> Lots { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
    }
}