namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostFloatTodecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Subscriptions", "Cost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Subscriptions", "Cost", c => c.Single(nullable: false));
        }
    }
}
