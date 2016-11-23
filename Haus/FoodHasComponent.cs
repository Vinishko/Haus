using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haus
{
    public class FoodHasComponent
    {
        [Key]
        [Column(Order = 1)]
        public int FoodId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ComponentId { get; set; }
        public double Amount { get; set; }

        public virtual Component Component { get; set; }
        public virtual Food Food { get; set; }
    }
}
