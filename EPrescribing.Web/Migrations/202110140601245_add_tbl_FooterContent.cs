namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_tbl_FooterContent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FooterContent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FooterText = c.String(nullable: false, maxLength: 100),
                        Facebook = c.String(maxLength: 150),
                        Twitter = c.String(maxLength: 150),
                        Tumblr = c.String(maxLength: 150),
                        Vimeo = c.String(maxLength: 150),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FooterContent");
        }
    }
}
