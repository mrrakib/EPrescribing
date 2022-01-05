namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PreviousPrescriptionIdMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prescriptions", "PreviousPrescriptionId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prescriptions", "PreviousPrescriptionId");
        }
    }
}
