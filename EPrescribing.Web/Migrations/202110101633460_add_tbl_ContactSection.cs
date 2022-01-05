namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_tbl_ContactSection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Address = c.String(nullable: false, maxLength: 300),
                        Icon = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ContactSections");
        }
    }
}
