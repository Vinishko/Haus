namespace Haus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_food_components : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Components",
                c => new
                    {
                        ComponentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UnitOfMeasurment = c.String(),
                    })
                .PrimaryKey(t => t.ComponentId);
            
            CreateTable(
                "dbo.FoodHasComponents",
                c => new
                    {
                        FoodId = c.Int(nullable: false),
                        ComponentId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.FoodId, t.ComponentId })
                .ForeignKey("dbo.Components", t => t.ComponentId, cascadeDelete: true)
                .ForeignKey("dbo.Foods", t => t.FoodId, cascadeDelete: true)
                .Index(t => t.FoodId)
                .Index(t => t.ComponentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FoodHasComponents", "FoodId", "dbo.Foods");
            DropForeignKey("dbo.FoodHasComponents", "ComponentId", "dbo.Components");
            DropIndex("dbo.FoodHasComponents", new[] { "ComponentId" });
            DropIndex("dbo.FoodHasComponents", new[] { "FoodId" });
            DropTable("dbo.FoodHasComponents");
            DropTable("dbo.Components");
        }
    }
}
