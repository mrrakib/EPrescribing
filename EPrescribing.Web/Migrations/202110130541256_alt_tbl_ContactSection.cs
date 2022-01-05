namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alt_tbl_ContactSection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactSections", "PhoneNo1", c => c.String(maxLength: 11));
            AddColumn("dbo.ContactSections", "PhoneNo2", c => c.String(maxLength: 11));
            AddColumn("dbo.ContactSections", "Email1", c => c.String(maxLength: 80));
            AddColumn("dbo.ContactSections", "Email2", c => c.String(maxLength: 80));
            AlterColumn("dbo.ContactSections", "Address", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContactSections", "Address", c => c.String(nullable: false, maxLength: 300));
            DropColumn("dbo.ContactSections", "Email2");
            DropColumn("dbo.ContactSections", "Email1");
            DropColumn("dbo.ContactSections", "PhoneNo2");
            DropColumn("dbo.ContactSections", "PhoneNo1");
        }
    }
}
