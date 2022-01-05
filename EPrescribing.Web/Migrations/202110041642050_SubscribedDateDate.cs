namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubscribedDateDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "SubscribedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "SubscribedDate");
        }
    }
}
