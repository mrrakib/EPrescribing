namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoseAmountFormulations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Medicines", "DoseAmount", c => c.String());
            AddColumn("dbo.Medicines", "Formulation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Medicines", "Formulation");
            DropColumn("dbo.Medicines", "DoseAmount");
        }
    }
}
