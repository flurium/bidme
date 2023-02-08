using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Models
{
    public class WaitingLot
    {
        public WaitingLot(int id, DateTime closeTime)
        {
            Id = id;
            CloseTime = closeTime;
        }

        public int Id { get; set; }
        public DateTime CloseTime { get; set; }
    }
}