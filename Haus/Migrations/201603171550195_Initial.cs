namespace Haus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        DiscountId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.DiscountId);
            
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        FoodId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                        FoodTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FoodId)
                .ForeignKey("dbo.FoodTypes", t => t.FoodTypeId, cascadeDelete: true)
                .Index(t => t.FoodTypeId);
            
            CreateTable(
                "dbo.FoodTypes",
                c => new
                    {
                        FoodTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.FoodTypeId);
            
            CreateTable(
                "dbo.OrderHasFoods",
                c => new
                    {
                        OrderId = c.Int(nullable: false),
                        FoodId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderId, t.FoodId })
                .ForeignKey("dbo.Foods", t => t.FoodId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.FoodId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                        Discount_DiscountId = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Discounts", t => t.Discount_DiscountId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.Discount_DiscountId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderHasFoods", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Orders", "Discount_DiscountId", "dbo.Discounts");
            DropForeignKey("dbo.OrderHasFoods", "FoodId", "dbo.Foods");
            DropForeignKey("dbo.Foods", "FoodTypeId", "dbo.FoodTypes");
            DropIndex("dbo.Orders", new[] { "User_UserId" });
            DropIndex("dbo.Orders", new[] { "Discount_DiscountId" });
            DropIndex("dbo.OrderHasFoods", new[] { "FoodId" });
            DropIndex("dbo.OrderHasFoods", new[] { "OrderId" });
            DropIndex("dbo.Foods", new[] { "FoodTypeId" });
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderHasFoods");
            DropTable("dbo.FoodTypes");
            DropTable("dbo.Foods");
            DropTable("dbo.Discounts");
        }
    }
}
