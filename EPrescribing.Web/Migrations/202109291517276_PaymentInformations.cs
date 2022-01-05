namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentInformations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "TotalPaid", c => c.Double(nullable: false));
            AddColumn("dbo.Patients", "TotalDue", c => c.Double(nullable: false));
            AddColumn("dbo.Patients", "TotalDiscount", c => c.Double(nullable: false));
            AddColumn("dbo.Payments", "TreatmentCharge", c => c.Double(nullable: false));
            AddColumn("dbo.Payments", "TotalDiscount", c => c.Double(nullable: false));
            DropColumn("dbo.Prescriptions", "TreatmentCharge");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prescriptions", "TreatmentCharge", c => c.Double(nullable: false));
            DropColumn("dbo.Payments", "TotalDiscount");
            DropColumn("dbo.Payments", "TreatmentCharge");
            DropColumn("dbo.Patients", "TotalDiscount");
            DropColumn("dbo.Patients", "TotalDue");
            DropColumn("dbo.Patients", "TotalPaid");
        }
    }
}
