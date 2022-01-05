namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class treatment_done_required_removed : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PrescribedMedicine", "MedicineId", "dbo.Medicines");
            DropIndex("dbo.PrescribedMedicine", new[] { "MedicineId" });
            AddColumn("dbo.PrescribedMedicine", "Days", c => c.String());
            AddColumn("dbo.PrescribedMedicine", "MedicineName", c => c.String());
            AlterColumn("dbo.TreatmentDones", "PathologyName", c => c.String());
            DropColumn("dbo.PrescribedMedicine", "MedicineId");
            DropColumn("dbo.PrescribedMedicine", "Dose");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PrescribedMedicine", "Dose", c => c.String());
            AddColumn("dbo.PrescribedMedicine", "MedicineId", c => c.Int(nullable: false));
            AlterColumn("dbo.TreatmentDones", "PathologyName", c => c.String(nullable: false));
            DropColumn("dbo.PrescribedMedicine", "MedicineName");
            DropColumn("dbo.PrescribedMedicine", "Days");
            CreateIndex("dbo.PrescribedMedicine", "MedicineId");
            AddForeignKey("dbo.PrescribedMedicine", "MedicineId", "dbo.Medicines", "Id", cascadeDelete: true);
        }
    }
}
