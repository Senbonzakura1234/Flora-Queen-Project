namespace Flora_Queen_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "UpdatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "DeletedAt", c => c.DateTime());
            AddColumn("dbo.Orders", "OrderStatus", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "CreatedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "CreatedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Orders", "OrderStatus");
            DropColumn("dbo.Orders", "DeletedAt");
            DropColumn("dbo.Orders", "UpdatedAt");
            DropColumn("dbo.Orders", "CreatedAt");
        }
    }
}
