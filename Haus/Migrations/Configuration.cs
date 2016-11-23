namespace Haus.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Haus.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Haus.Context";
        }

        protected override void Seed(Haus.Context context)
        {
            context.Users.AddOrUpdate(u=>u.Login, new User{IsAdmin = true,Login = "admin",Name = "Administrtor",Password = "1111"});
            context.Discounts.AddOrUpdate(d=>d.DiscountId, new Discount{Name = "Без знижки",Percent = 0, SumFrom=0,SumTo = Int32.MaxValue-2,Value = 1});
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
