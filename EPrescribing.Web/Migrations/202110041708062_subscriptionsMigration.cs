namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscriptionsMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Subscriptions", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Subscriptions", "Name", c => c.String());
        }
    }
}
