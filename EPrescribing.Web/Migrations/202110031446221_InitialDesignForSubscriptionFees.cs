namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDesignForSubscriptionFees : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubscriptionFees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        SubscriptionId = c.Int(nullable: false),
                        PaymentMethod = c.String(),
                        TransactionNo = c.String(),
                        Reference = c.String(),
                        DebitAccount = c.String(),
                        CreditAccount = c.String(),
                        PayableAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SubscriptionFees");
        }
    }
}
