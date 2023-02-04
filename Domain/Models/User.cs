using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
  public class User : IdentityUser
  {
    public ICollection<Lot> Lots { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
    public ICollection<Rating> Ratings { get; set; }
  }
}