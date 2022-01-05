namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class diseasename_trearmentname_required_remove : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DifferentialDiagnosis", "DiseaseName", c => c.String());
            AlterColumn("dbo.TreatmentPlans", "TreatmentName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TreatmentPlans", "TreatmentName", c => c.String(nullable: false));
            AlterColumn("dbo.DifferentialDiagnosis", "DiseaseName", c => c.String(nullable: false));
        }
    }
}
