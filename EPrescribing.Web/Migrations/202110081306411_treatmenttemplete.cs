namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class treatmenttemplete : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TreatmentTempletes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        TreatmentName = c.String(),
                        PrescribedMedicine = c.String(),
                        Advice = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TreatmentTempletes", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.TreatmentTempletes", new[] { "DoctorId" });
            DropTable("dbo.TreatmentTempletes");
        }
    }
}
