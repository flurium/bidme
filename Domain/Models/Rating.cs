using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
  public class Rating
  {
    public int Id { get; set; }
    public double Value { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public bool AsSeller { get; set; }
  }
}