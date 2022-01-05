namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doctor_isbmdcverified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "IsBMDCVerified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "IsBMDCVerified");
        }
    }
}
