namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeOneToManyPaymentRel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payments", "PrescriptionId", "dbo.Prescriptions");
            DropIndex("dbo.Payments", new[] { "PrescriptionId" });
            AddColumn("dbo.Prescriptions", "PaymentId", c => c.Int());
            CreateIndex("dbo.Prescriptions", "PaymentId");
            AddForeignKey("dbo.Prescriptions", "PaymentId", "dbo.Payments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "PaymentId", "dbo.Payments");
            DropIndex("dbo.Prescriptions", new[] { "PaymentId" });
            DropColumn("dbo.Prescriptions", "PaymentId");
            CreateIndex("dbo.Payments", "PrescriptionId");
            AddForeignKey("dbo.Payments", "PrescriptionId", "dbo.Prescriptions", "Id", cascadeDelete: true);
        }
    }
}
