namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TotalDueAmountMigrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "TotalDueAmount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "TotalDueAmount");
        }
    }
}
