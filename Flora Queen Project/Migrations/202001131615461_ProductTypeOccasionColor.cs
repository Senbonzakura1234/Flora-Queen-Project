namespace Flora_Queen_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTypeOccasionColor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        ProductId = c.String(nullable: false, maxLength: 128),
                        OrderId = c.String(nullable: false, maxLength: 128),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.OrderId })
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                        InStock = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                        TypeId = c.String(maxLength: 128),
                        OccasionId = c.String(maxLength: 128),
                        ColorId = c.String(maxLength: 128),
                        ProductStatus = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colors", t => t.ColorId)
                .ForeignKey("dbo.Occasions", t => t.OccasionId)
                .ForeignKey("dbo.Types", t => t.TypeId)
                .Index(t => t.TypeId)
                .Index(t => t.OccasionId)
                .Index(t => t.ColorId);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                        ColorStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Occasions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                        OccasionStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        DeletedAt = c.DateTime(),
                        TypeStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "ApplicationUserId");
            AddForeignKey("dbo.Orders", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "TypeId", "dbo.Types");
            DropForeignKey("dbo.OrderItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "OccasionId", "dbo.Occasions");
            DropForeignKey("dbo.Products", "ColorId", "dbo.Colors");
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Products", new[] { "ColorId" });
            DropIndex("dbo.Products", new[] { "OccasionId" });
            DropIndex("dbo.Products", new[] { "TypeId" });
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            DropIndex("dbo.OrderItems", new[] { "ProductId" });
            DropIndex("dbo.Orders", new[] { "ApplicationUserId" });
            DropColumn("dbo.Orders", "ApplicationUserId");
            DropTable("dbo.Types");
            DropTable("dbo.Occasions");
            DropTable("dbo.Colors");
            DropTable("dbo.Products");
            DropTable("dbo.OrderItems");
        }
    }
}
