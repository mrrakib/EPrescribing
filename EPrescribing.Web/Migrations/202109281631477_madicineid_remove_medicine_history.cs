namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class madicineid_remove_medicine_history : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MedicineHistories", "MedicineId", "dbo.Medicines");
            DropIndex("dbo.MedicineHistories", new[] { "MedicineId" });
            AddColumn("dbo.MedicineHistories", "MedicineName", c => c.String());
            DropColumn("dbo.MedicineHistories", "MedicineId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MedicineHistories", "MedicineId", c => c.Int(nullable: false));
            DropColumn("dbo.MedicineHistories", "MedicineName");
            CreateIndex("dbo.MedicineHistories", "MedicineId");
            AddForeignKey("dbo.MedicineHistories", "MedicineId", "dbo.Medicines", "Id", cascadeDelete: true);
        }
    }
}
