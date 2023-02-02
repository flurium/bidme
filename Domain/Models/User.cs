using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
