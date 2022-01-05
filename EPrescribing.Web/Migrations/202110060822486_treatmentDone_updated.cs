namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class treatmentDone_updated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TreatmentDones", "TreatmentName", c => c.String());
            DropColumn("dbo.TreatmentDones", "PathologyName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TreatmentDones", "PathologyName", c => c.String());
            DropColumn("dbo.TreatmentDones", "TreatmentName");
        }
    }
}
