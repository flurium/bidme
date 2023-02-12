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

            builder.Entity<Favorite>().HasKey(f => new { f.UserId, f.LotId });
            builder.Entity<Favorite>().HasOne(f => f.User).WithMany(u => u.Favorites).HasForeignKey(f => f.UserId);
            builder.Entity<Favorite>().HasOne(f => f.Lot).WithMany(l => l.Favorites).HasForeignKey(f => f.LotId);

            builder.Entity<Order>().HasKey(o => new { o.UserId, o.LotId });
            builder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);
            builder.Entity<Order>().HasOne(o => o.Lot).WithMany(l => l.Orders).HasForeignKey(o => o.LotId);

            builder.Entity<Rating>().HasKey(r => r.Id);
            builder.Entity<Rating>().HasOne(r => r.User).WithMany(u => u.Ratings).HasForeignKey(r => r.UserId);

            builder.Entity<Lot>().HasKey(l => l.Id);
            builder.Entity<Lot>().HasOne(l => l.Category).WithMany(c => c.Lots).HasForeignKey(l => l.CategoryId);
            builder.Entity<Lot>().HasOne(l => l.User).WithMany(u => u.Lots).HasForeignKey(l => l.UserId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);

            builder.Entity<LotImage>().HasKey(li => li.Id);
            builder.Entity<LotImage>().HasOne(li => li.Lot).WithMany(l => l.Images).HasForeignKey(l => l.LotId);

            builder.Entity<Category>().HasKey(c => c.Id);
            builder.Entity<Category>().HasOne(c => c.Parent).WithMany(c => c.Subcategories).HasForeignKey(c => c.ParentId);
        }
    }
}