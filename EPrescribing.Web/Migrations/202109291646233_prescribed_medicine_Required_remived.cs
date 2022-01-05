namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prescribed_medicine_Required_remived : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PrescribedAdvices", "AdviceName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PrescribedAdvices", "AdviceName", c => c.String(nullable: false));
        }
    }
}
