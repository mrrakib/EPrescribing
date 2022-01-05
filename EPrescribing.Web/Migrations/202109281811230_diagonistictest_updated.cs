namespace EPrescribing.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class diagonistictest_updated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DiagnosticTest", "DiagnosticId", "dbo.Diagnostics");
            DropIndex("dbo.DiagnosticTest", new[] { "DiagnosticId" });
            AddColumn("dbo.DiagnosticTest", "DiagnosticName", c => c.String());
            DropColumn("dbo.DiagnosticTest", "DiagnosticId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DiagnosticTest", "DiagnosticId", c => c.Int(nullable: false));
            DropColumn("dbo.DiagnosticTest", "DiagnosticName");
            CreateIndex("dbo.DiagnosticTest", "DiagnosticId");
            AddForeignKey("dbo.DiagnosticTest", "DiagnosticId", "dbo.Diagnostics", "Id", cascadeDelete: true);
        }
    }
}
