using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class LotImage
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int LotId { get; set; }
        public Lot Lot { get; set; }
    }
}
