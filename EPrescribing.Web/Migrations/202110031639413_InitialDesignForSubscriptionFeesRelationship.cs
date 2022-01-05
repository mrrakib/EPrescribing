namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDesignForSubscriptionFeesRelationship : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.SubscriptionFees", "DoctorId");
            CreateIndex("dbo.SubscriptionFees", "SubscriptionId");
            AddForeignKey("dbo.SubscriptionFees", "DoctorId", "dbo.Doctors", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SubscriptionFees", "SubscriptionId", "dbo.Subscriptions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscriptionFees", "SubscriptionId", "dbo.Subscriptions");
            DropForeignKey("dbo.SubscriptionFees", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.SubscriptionFees", new[] { "SubscriptionId" });
            DropIndex("dbo.SubscriptionFees", new[] { "DoctorId" });
        }
    }
}
