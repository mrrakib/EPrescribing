namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NextAppointmentScheduleMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Prescriptions", "NextAppointmentSchedule");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Prescriptions", "NextAppointmentSchedule", c => c.String());
        }
    }
}
