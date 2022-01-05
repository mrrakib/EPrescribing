namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDesignForSubscriptionFeesRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubscriptionFees", "PaymentMethod", c => c.String(nullable: false));
            AlterColumn("dbo.SubscriptionFees", "TransactionNo", c => c.String(nullable: false));
            AlterColumn("dbo.SubscriptionFees", "DebitAccount", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubscriptionFees", "DebitAccount", c => c.String());
            AlterColumn("dbo.SubscriptionFees", "TransactionNo", c => c.String());
            AlterColumn("dbo.SubscriptionFees", "PaymentMethod", c => c.String());
        }
    }
}
