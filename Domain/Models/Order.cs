using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Order
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int LotId { get; set; }
        public Lot Lot { get; set; }

    }
}
