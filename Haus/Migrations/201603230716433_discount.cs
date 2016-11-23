namespace Haus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class discount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DiscountCards",
                c => new
                    {
                        DiscountCardId = c.Int(nullable: false, identity: true),
                        HolderName = c.String(),
                        TotSum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DiscountCardId);
            
            AddColumn("dbo.Discounts", "SumFrom", c => c.Int(nullable: false));
            AddColumn("dbo.Discounts", "SumTo", c => c.Int(nullable: false));
            AddColumn("dbo.Discounts", "Percent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Discounts", "Percent");
            DropColumn("dbo.Discounts", "SumTo");
            DropColumn("dbo.Discounts", "SumFrom");
            DropTable("dbo.DiscountCards");
        }
    }
}
