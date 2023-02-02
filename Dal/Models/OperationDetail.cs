using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Models
{
    public class OperationDetail
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public bool IsError { get; set; }
    }
}