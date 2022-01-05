namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NextAppointmentDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "NextAppointmentDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patients", "NextAppointmentDate");
        }
    }
}
