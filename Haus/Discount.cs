using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haus
{
    public class Discount
    {
        public int DiscountId { get; set; }
        public string Name { get; set; }
        public int SumFrom { get; set; }
        public int SumTo { get; set; }
        public int Percent { get; set; }
        public double Value { get; set; }
    }
}
