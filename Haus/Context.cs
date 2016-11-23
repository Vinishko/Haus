using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haus
{
    public class Context:DbContext
    {
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodType> FoodTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderHasFood> OrderHasFoods { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DiscountCard> DiscountCards { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<FoodHasComponent> FoodHasComponents { get; set; } 
         
    }
}
