namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BugFixingIssues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Generics", "DoseAmount", c => c.String());
            AddColumn("dbo.Generics", "Formulation", c => c.String());
            AlterColumn("dbo.ContactSections", "PhoneNo1", c => c.String(maxLength: 20));
            AlterColumn("dbo.ContactSections", "PhoneNo2", c => c.String(maxLength: 20));
            AlterColumn("dbo.SingleServiceSections", "Icon", c => c.String(maxLength: 25));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SingleServiceSections", "Icon", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.ContactSections", "PhoneNo2", c => c.String(maxLength: 11));
            AlterColumn("dbo.ContactSections", "PhoneNo1", c => c.String(maxLength: 11));
            DropColumn("dbo.Generics", "Formulation");
            DropColumn("dbo.Generics", "DoseAmount");
        }
    }
}
