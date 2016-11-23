using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haus
{
   public class Order
    {
        public int OrderId { get; set; }
        public Status Status{ get; set; }
        public DateTime Time { get; set; }
        public Discount Discount { get; set; }
        public User User { get; set; }
        public double OrderSum { get; set; }
    }

    public enum Status
    {
        Ordered,
        Working,
        Canceled,
        Closed
    }
}
