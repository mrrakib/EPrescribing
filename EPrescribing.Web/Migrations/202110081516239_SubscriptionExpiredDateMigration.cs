namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscriptionExpiredDateMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "SubscriptionExpiredDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "SubscriptionExpiredDate");
        }
    }
}
