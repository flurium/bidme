using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Models
{
  public class LotFilter
  {
    public string? Name { get; set; }
    public List<string> Categories { get; set; }
  }
}