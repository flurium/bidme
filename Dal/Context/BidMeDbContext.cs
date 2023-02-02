using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Context
{
  public class BidMeDbContext : IdentityDbContext
  {
    public BidMeDbContext(DbContextOptions options) : base(options)
    {
      Database.EnsureCreated();
    }
  }
}