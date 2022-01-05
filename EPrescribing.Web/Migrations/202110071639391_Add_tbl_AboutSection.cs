namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_tbl_AboutSection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AboutSections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TopTitle = c.String(nullable: false, maxLength: 100),
                        MainTitle = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ListOneTitle = c.String(nullable: false, maxLength: 100),
                        ListOneDescription = c.String(nullable: false),
                        ListOneIcon = c.String(nullable: false, maxLength: 50),
                        ListSecondTitle = c.String(nullable: false, maxLength: 100),
                        ListSecondDescription = c.String(nullable: false),
                        ListSecondIcon = c.String(nullable: false, maxLength: 50),
                        ImagePath = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AboutSections");
        }
    }
}
