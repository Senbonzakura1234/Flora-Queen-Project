namespace Flora_Queen_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Amount = c.Double(nullable: false),
                        OrderDescription = c.String(),
                        BankCode = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        vnp_TransactionNo = c.Int(nullable: false),
                        vpn_Message = c.String(),
                        vpn_TxnResponseCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Orders");
        }
    }
}
