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
      builder.Entity<Favorite>().HasOne(f => f.User).WithMany(u => u.Favorites).HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.NoAction);
      builder.Entity<Favorite>().HasOne(f => f.Lot).WithMany(l => l.Favorites).HasForeignKey(f => f.LotId);

      builder.Entity<Order>().HasKey(o => new { o.UserId, o.LotId });
      builder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.NoAction);
      builder.Entity<Order>().HasOne(o => o.Lot).WithMany(l => l.Orders).HasForeignKey(o => o.LotId).OnDelete(DeleteBehavior.NoAction);

      builder.Entity<Rating>().HasKey(r => r.Id);
    }
  }
}