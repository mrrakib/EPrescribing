namespace EPrescribing.Web.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PaymentsList : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Prescriptions", "PaymentId", "dbo.Payments");
            DropIndex("dbo.Prescriptions", new[] { "PaymentId" });
            CreateIndex("dbo.Payments", "PrescriptionId");
            AddForeignKey("dbo.Payments", "PrescriptionId", "dbo.Prescriptions", "Id", cascadeDelete: false);
            DropColumn("dbo.Prescriptions", "PaymentId");
        }

        public override void Down()
        {
            AddColumn("dbo.Prescriptions", "PaymentId", c => c.Int());
            DropForeignKey("dbo.Payments", "PrescriptionId", "dbo.Prescriptions");
            DropIndex("dbo.Payments", new[] { "PrescriptionId" });
            CreateIndex("dbo.Prescriptions", "PaymentId");
            AddForeignKey("dbo.Prescriptions", "PaymentId", "dbo.Payments", "Id");
        }
    }
}
