using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haus
{
    public class Food
    {
        public int FoodId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public int FoodTypeId { get; set; }

        public virtual FoodType FoodType{ get; set; }
        
    }
}
