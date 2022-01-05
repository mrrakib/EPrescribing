namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class treatmenttemplete_presciption : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prescriptions", "MedicinePrescribed", c => c.String());
            AddColumn("dbo.Prescriptions", "AdvicePrescribed", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prescriptions", "AdvicePrescribed");
            DropColumn("dbo.Prescriptions", "MedicinePrescribed");
        }
    }
}
