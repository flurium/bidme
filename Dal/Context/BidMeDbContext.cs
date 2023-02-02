using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dal.Context
{
    public class BidMeDbContext : IdentityDbContext<User>
    {
        public BidMeDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<Lot> Lots { get; set; }
        public DbSet<LotImage> LotImages { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}