namespace Haus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class X : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderSum", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderSum");
        }
    }
}
